using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    public class OrderDeletedEvent : BaseEvent
    {
        public Order Order { get; set; }

        public OrderDeletedEvent(Order order)
        {
            Order = order;
        }
    }
}
