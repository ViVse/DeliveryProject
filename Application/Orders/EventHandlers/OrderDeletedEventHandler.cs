using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Orders.EventHandlers
{
    public class OrderDeletedEventHandler : INotificationHandler<OrderDeletedEvent>
    {
        private readonly ILogger<OrderDeletedEventHandler> _logger;

        public OrderDeletedEventHandler(ILogger<OrderDeletedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
