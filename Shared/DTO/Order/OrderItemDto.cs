namespace Shared.DTO.Order
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string ImageFilePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid AddressId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public Guid ShippingCostId { get; set; }
        public string OrderCode { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool OrderStatus { get; set; }

        public string Address{ get; set; }


        public IEnumerable<OrderItemDto> OrderItems { get; set; }

        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalAmount { get; set; }
        public string Note { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
    public class UpdateOrderDto
    {
        public bool OrderStatus { get; set; }
    }
}
