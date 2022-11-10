using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;
using MongoDB.Driver;

namespace Application.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand(string Id): IRequest;

    public class DeleteOrderCommandHandler: IRequestHandler<DeleteOrderCommand>
    {
        private readonly IMongoDbContext _context;

        public DeleteOrderCommandHandler(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var collection = _context.ConnectToMongo<Order>("orders");
            var deleteFilter = Builders<Order>.Filter.Eq("Id", command.Id);
            var cursor = await collection.FindAsync(deleteFilter, null, cancellationToken);
            cursor.MoveNext();
            if (!cursor.Current.Any())
            {
                throw new NotFoundException(nameof(Order), command.Id);
            }
            var entity = cursor.Current.First();
            entity.AddDomainEvent(new OrderDeletedEvent(entity));
            await collection.DeleteOneAsync(deleteFilter, cancellationToken);
            await _context.PublishEvents(entity.DomainEvents);

            return Unit.Value;
        }
    }
}
