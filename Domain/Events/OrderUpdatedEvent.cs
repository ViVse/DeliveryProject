using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    public class OrderUpdatedEvent : BaseEvent
    {
        public Order Order { get; set; }

        public OrderUpdatedEvent(Order order)
        {
            Order = order;
        }
    }
}
