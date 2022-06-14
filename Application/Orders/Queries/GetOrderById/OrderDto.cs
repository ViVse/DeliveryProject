using Application.Common.Mappings;
using Domain.Entities;
using Domain.Enums;

namespace Application.Orders.Queries.GetOrderById
{
    public class OrderDto: IMapFrom<Order>
    {
        public string Id { get; set; }
        public Customer Customer { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public DeliveryMan DeliveryMan { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public Address Address { get; set; }
        public List<Product> Products { get; set; }
    }
}