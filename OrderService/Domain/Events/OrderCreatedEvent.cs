using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    public class OrderCreatedEvent: BaseEvent
    {
        public Order Order { get; set; }

        public OrderCreatedEvent(Order order)
        {
            Order = order;
        }
    }
}
