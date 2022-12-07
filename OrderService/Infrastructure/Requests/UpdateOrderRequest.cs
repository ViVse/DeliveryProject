using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Requests
{
    public class UpdateOrderRequest
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public int DeliveryManId { get; set; }
        public DateTime Date { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public string AddressLine { get; set; }
        public List<Product> Products { get; set; }
        public float TotalPrice { get; set; }
    }
}
