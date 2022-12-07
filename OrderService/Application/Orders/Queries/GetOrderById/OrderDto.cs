using Application.Common.Mappings;
using Domain.Entities;
using Domain.Enums;

namespace Application.Orders.Queries.GetOrderById
{
    public class OrderDto: IMapFrom<Order>
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public int DeliveryManId { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public string AddressLine { get; set; }
        public List<Product> Products { get; set; }
    }
}