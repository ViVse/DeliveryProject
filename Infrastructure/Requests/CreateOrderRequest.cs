using Domain.Enums;

namespace Infrastructure.Requests
{
    public class CreateOrderRequest
    {
        public string CustomerId { get; set; }
        public int DeliveryManId { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public int AddressId { get; set; }
        public Dictionary<int, int> Products { get; set; }
    }
}
