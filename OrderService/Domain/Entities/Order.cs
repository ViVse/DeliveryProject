using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Order: BaseEntity
    {
        public string UserId { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public int DeliveryManId { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public string AddressLine { get; set; }
        public List<Product> Products { get; set; }
    }
}
