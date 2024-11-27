using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Order;
using Shared.DTO.Response;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public OrderController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {

                // Tạo mã đơn hàng dựa trên thời gian và chuỗi ngẫu nhiên
                var orderCode = GenerateOrderCode();

                // Lấy danh sách sản phẩm trong giỏ hàng
                var cartItems = await _repository.Cart.GetCartItemsByUserIdAsync(createOrderDto.UserId, trackChanges: false);
                if (cartItems == null || !cartItems.Any())
                {
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Không tìm thấy giỏ hàng của người dùng này {createOrderDto.UserId}!",
                        Data = null
                    });
                }

                var totalDiscount = cartItems.Sum(item => item.Discount * item.Quantity);
                var totalPrice = cartItems.Sum(item => item.Price * item.Quantity);


                // Tạo đợn hàng mới
                var order = _mapper.Map<Order>(createOrderDto);

                order.Id = Guid.NewGuid();
                order.OrderCode = orderCode;
                order.Discount = totalDiscount;
                order.Price = totalPrice;
                order.OrderStatus = false;
                order.TotalAmount = totalPrice - totalDiscount;

                order.OrderDate = DateTime.UtcNow;


                var orderItems = cartItems.Select(item => new OrderItemDto
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Name,
                    CategoryName = item.CategoryName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Discount = item.Discount,
                    ImageFilePath = item.ImageFilePath
                }).ToList();

                //Map giỏ hàng vào orderItem
                var orderItemEntities = _mapper.Map<IEnumerable<OrderItem>>(orderItems);

                order.OrderItems = orderItemEntities.ToList();


                await _repository.Order.CreateOrderAsync(order);
                await _repository.Order.SaveAsync();


                // Xoá giỏ hàng và lưu
                await _repository.Cart.DeleteCartItemsByUserIdAsync(order.UserId);
                await _repository.Cart.SaveAsync();

                return Ok(new ApiResponse<OrderDto>
                {
                    Success = true,
                    Message = "Order created successfully.",
                    Data = _mapper.Map<OrderDto>(order)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Đã xảy ra lỗi trong CreateOrder: {ex.Message}");
                return StatusCode(500, new ApiResponse<Object>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi nội bộ.",
                    Data = null
                });
            }
        }

        [HttpGet]
        [Route("GetOrdersList")]
        public async Task<IActionResult> GetOrdersList([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] int? type = null, [FromQuery] string? orderCode = null) // type = 0, 1, 2 (All order, true, false)
        {
            try
            {

                var (orders, totalCount) = await _repository.Order.GetOrdersListAsync(pageNumber, pageSize, type, orderCode);

                if (orders == null || !orders.Any())
                {
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = "Không tìm thấy đơn hàng nào cho người dùng này.",
                        Data = null
                    });
                }

                var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);

                return Ok(new
                {
                    success = true,
                    message = "Products retrieved successfully.",
                    data = new
                    {
                        totalCount,
                        orders = orderDtos
                    }
                });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Đã xảy ra lỗi khi lấy danh sách đơn hàng: {ex.Message}");
                return StatusCode(500, new ApiResponse<Object>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xử lý yêu cầu.",
                    Data = null
                });
            }
        }

        [HttpGet]
        [Route("GetOrderById/{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _repository.Order.GetOrderByIdAsync(orderId, trackChanges: false);

            if (order == null)
            {
                return NotFound(new ApiResponse<Object>
                {
                    Success = false,
                    Message = "Order not found.",
                    Data = null
                });
            }

            var orderDto = _mapper.Map<OrderDto>(order);
            orderDto.OrderCode = order.OrderCode;

            return Ok(new ApiResponse<OrderDto>
            {
                Success = true,
                Message = "Order details retrieved successfully.",
                Data = orderDto
            });
        }

        [HttpDelete]
        [Route("DeleteOrder/{Id}")]
        public async Task<IActionResult> DeleteOrder(Guid Id)
        {
            try
            {
                var order = await _repository.Order.GetOrderByIdAsync(Id, trackChanges: false);
                if (order == null)
                {
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Không tìm thấy đơn hàng với ID {Id}!",
                        Data = null
                    });
                }


                _repository.Order.DeleteOrderAsync(order);
                _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteBrand action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("UpdateOrderStatus/{Id}")]
        public async Task<IActionResult> UpdateOrderStatus(Guid Id, [FromQuery] UpdateOrderDto updateOrderDto)
        {
            try
            {
                var order = await _repository.Order.GetOrderByIdAsync(Id, trackChanges: false);
                if (order == null)
                {
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Order with id {Id} not found.",
                        Data = null
                    });
                }

                order.OrderStatus = updateOrderDto.OrderStatus;

                await _repository.Order.UpdateOrderAsync(order);
                await _repository.Order.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong while updating order status: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetListOrdersType")]
        public async Task<IActionResult> GetListOrdersType([FromQuery] int type = 0) // 0 tất cả, 1 là đơn hàng true, 2 là false
        {
            try
            {
                var orders = await _repository.Order.GetAllOrdersAsync(type, trackChanges: false);
                if (orders == null || !orders.Any())
                {
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = "No pending orders found.",
                        Data = null
                    });
                }

                var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
                return Ok(new ApiResponse<IEnumerable<OrderDto>>
                {
                    Success = true,
                    Message = "Pending orders retrieved successfully.",
                    Data = ordersDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong while getting pending orders: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private string GenerateOrderCode()
        {
            var random = new Random();
            var orderCode = random.Next(1000, 9999); // Tạo chuỗi ngẫu nhiên gồm 8 số

            return $"OD{orderCode}";
        }
    }
}
