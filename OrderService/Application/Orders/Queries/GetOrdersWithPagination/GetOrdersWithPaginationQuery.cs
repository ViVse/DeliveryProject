using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Orders.Queries.GetOrdersWithPagination
{
    public record GetOrdersWithPaginationQuery: IRequest<PaginatedList<OrderBriefDto>>
    {
        public string? CustomerId { get; init; }
        public OrderStatusEnum? OrderStatus { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; }= 10;
    }

    public class GetOrdersWithPaginationQueryHandler: IRequestHandler<GetOrdersWithPaginationQuery, PaginatedList<OrderBriefDto>>
    {
        private readonly IMongoDbContext _context;
        private readonly IMapper _mapper;

        public GetOrdersWithPaginationQueryHandler(IMongoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<OrderBriefDto>> Handle(GetOrdersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var countFacet = AggregateFacet.Create("countFacet",
            PipelineDefinition<Order, AggregateCountResult>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Count<Order>()
            }));

            // data facet, we’ll use this to sort the data and do the skip and limiting of the results for the paging.
            var dataFacet = AggregateFacet.Create("dataFacet",
                PipelineDefinition<Order, Order>.Create(new[]
                {
                PipelineStageDefinitionBuilder.Sort(Builders<Order>.Sort.Ascending(x => x.Date)),
                PipelineStageDefinitionBuilder.Skip<Order>((request.PageNumber - 1) * request.PageSize),
                PipelineStageDefinitionBuilder.Limit<Order>(request.PageSize),
                }));

            var customerFilter = request.CustomerId is null || request.CustomerId.Trim() == String.Empty ?
                Builders<Order>.Filter.Empty 
                : Builders<Order>.Filter.Eq("UserId", request.CustomerId);
            var orderStatusFilter = request.OrderStatus is null ?
                 Builders<Order>.Filter.Empty
                : Builders<Order>.Filter.Eq("OrderStatus", request.OrderStatus);
            var filter = Builders<Order>.Filter.And(customerFilter, orderStatusFilter);

            var collection = _context.ConnectToMongo<Order>("orders");
            var aggregation = await collection.Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "countFacet")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var rawData = aggregation.First()
                .Facets.First(x => x.Name == "dataFacet")
                .Output<Order>();

            var data = rawData.AsQueryable().ProjectTo<OrderBriefDto>(_mapper.ConfigurationProvider);

            return new PaginatedList<OrderBriefDto>(data, (int)count, request.PageNumber, request.PageSize);
        }
    }
}
