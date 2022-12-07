using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using MediatR;

namespace Application.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand: IRequest<string>
    {
        public string UserId { get; set; }
        public float TotalPrice { get; set; }
        public int DeliveryManId { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public string AddressLine { get; set; }
        public List<Product> Products { get; set; }
    }

    public class CreateOrderCommandHandler: IRequestHandler<CreateOrderCommand, string>
    {
        private readonly IMongoDbContext _context;

        public CreateOrderCommandHandler(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = new Order
            {
                UserId = request.UserId,
                TotalPrice = request.TotalPrice,
                Date = DateTime.Now,
                DeliveryManId = request.DeliveryManId,
                OrderStatus = request.OrderStatus,
                AddressLine = request.AddressLine,
                Products = request.Products
            };

            entity.AddDomainEvent(new OrderCreatedEvent(entity));

            await _context.ConnectToMongo<Order>("orders").InsertOneAsync(entity, cancellationToken);
            await _context.PublishEvents(entity.DomainEvents);

            return entity.Id;
        }
    }
}
