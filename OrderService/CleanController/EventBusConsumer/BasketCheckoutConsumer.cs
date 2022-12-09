using Application.Orders.Commands.CreateOrder;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(IMediator mediator, IMapper mapper, ILogger<BasketCheckoutConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            CreateOrderCommand command = new CreateOrderCommand()
            {
                UserId = context.Message.UserId,
                TotalPrice = (float)context.Message.TotalPrice,
                AddressLine = context.Message.AddressLine,
                Products = context.Message.Products.Select(_mapper.Map<Product, Domain.Entities.Product>).ToList(),
                OrderStatus = Domain.Enums.OrderStatusEnum.Pending,
            };
            await _mediator.Send(command);
        }
    }
}
