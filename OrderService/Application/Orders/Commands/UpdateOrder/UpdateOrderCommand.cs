using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand : IRequest<string>
    {
        public string Id { get; set;}
        public string UserId { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public int DeliveryManId { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public string AddressLine { get; set; }
        public List<Product> Products { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, string>
    {
        private readonly IMongoDbContext _context;

        public UpdateOrderCommandHandler(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = new Order
            {
                Id = request.Id,
                UserId = request.UserId,
                TotalPrice = request.TotalPrice,
                Date = DateTime.Now,
                DeliveryManId = request.DeliveryManId,
                OrderStatus = request.OrderStatus,
                AddressLine = request.AddressLine,
                Products = request.Products
            };

            entity.AddDomainEvent(new OrderUpdatedEvent(entity));

            var filter = Builders<Order>.Filter.Eq("Id", request.Id);
            await _context.ConnectToMongo<Order>("orders").ReplaceOneAsync(filter, entity, new UpdateOptions { IsUpsert = false }, cancellationToken);
            await _context.PublishEvents(entity.DomainEvents);

            return entity.Id;
        }
    }
}
