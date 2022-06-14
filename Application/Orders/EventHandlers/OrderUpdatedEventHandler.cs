using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Orders.EventHandlers
{
    public class OrderUpdatedEventHandler : INotificationHandler<OrderUpdatedEvent>
    {
        private readonly ILogger<OrderUpdatedEventHandler> _logger;

        public OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
