using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Order: BaseEntity
    {
        public Customer Customer { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public DeliveryMan DeliveryMan { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Address Address { get; set; }
        public List<Product> Products { get; set; }
    }
}
