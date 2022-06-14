using Application.Common.Mappings;
using Domain.Entities;
using Domain.Enums;

namespace Application.Orders.Queries.GetOrdersWithPagination
{
    public class OrderBriefDto: IMapFrom<Order>
    {
        public string Id { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public string OrderStatus { get; set; }
    }
}
