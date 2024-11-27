using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly RepositoryContext _dbContext;

        public OrderRepository(RepositoryContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            await _dbContext.Orders.AddAsync(order);
            return order;
        }

        public void DeleteOrderAsync(Order order)
        {
            Delete(order);
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId, bool trackChanges)
        {
            return await FindByCondition(order => order.Id.Equals(orderId), trackChanges)
                .Include(order => order.OrderItems)
                .FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<Order> Orders, int Total)> GetOrdersListAsync(int pageNumber, int pageSize, int? type = null, string? orderCode = null)
        {
            IQueryable<Order> ordersQuery = _dbContext.Orders
             .AsNoTracking()
             .Include(o => o.CartItems)
             .ThenInclude(x => x.Product);

            if (!string.IsNullOrEmpty(orderCode))
            {
                string lowerCaseName = orderCode.ToLower();

                ordersQuery = ordersQuery.Where(x => x.OrderCode.ToLower().Contains(lowerCaseName));
            }

            if (type == 1)
            {
                ordersQuery = ordersQuery.Where(x => x.OrderStatus);
            }
            else if (type == 2)
            {
                ordersQuery = ordersQuery.Where(x => !x.OrderStatus);
            }

            var totalOrders = await ordersQuery.CountAsync();

            // Thực hiện phân trang
            var orders = await ordersQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, totalOrders);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync(int type, bool trackChanges)
        {

            IQueryable<Order> ordersQuery = _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.CartItems)
                .ThenInclude(x => x.Product);

            // Lọc theo type
            if (type == 1)
            {
                ordersQuery = ordersQuery.Where(x => x.OrderStatus);
            }
            else if (type == 2)
            {
                ordersQuery = ordersQuery.Where(x => !x.OrderStatus);
            }

            return await ordersQuery.ToListAsync();
        }


        public async Task UpdateOrderAsync(Order order)
        {
            _dbContext.Orders.Update(order);
        }
    }
}
