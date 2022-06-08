using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using MediatR;

namespace Application.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand: IRequest<string>
    {
        public Customer Customer { get; set; }
        public float TotalPrice { get; set; }
        public DeliveryMan DeliveryMan { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Address Address { get; set; }
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
                Customer = request.Customer,
                TotalPrice = request.TotalPrice,
                Date = DateTime.Now,
                DeliveryMan = request.DeliveryMan,
                OrderStatus = request.OrderStatus,
                Address = request.Address,
                Products = request.Products
            };

            entity.AddDomainEvent(new OrderCreatedEvent(entity));


            await _context.ConnectToMongo<Order>("orders").InsertOneAsync(entity, cancellationToken);
            await _context.PublishEvents(entity.DomainEvents);

            return entity.Id;
        }
    }
}
