using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace Application.Orders.Queries.GetOrderById
{
    public record GetOrderByIdQuery(string Id) : IRequest<OrderDto>;

    public class GetOrderByIdQueryHandler: IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IMongoDbContext _context;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IMongoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var collection = _context.ConnectToMongo<Order>("orders");
            var filter = Builders<Order>.Filter.Eq("Id", request.Id);
            var cursor = await collection.FindAsync(filter, null, cancellationToken);
            cursor.MoveNext();
            if (!cursor.Current.Any())
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }
            var entity = cursor.Current.First();  
            return _mapper.Map<Order, OrderDto>(entity);
        }
    }
}
