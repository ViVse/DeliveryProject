using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Order: BaseEntity
    {
        public string? CustomerId { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public int DeliveryManId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int AddressId { get; set; }
    }
}
