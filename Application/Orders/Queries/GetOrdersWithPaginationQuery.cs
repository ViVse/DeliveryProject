using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace Application.Orders.Queries
{
    public record GetOrdersWithPaginationQuery: IRequest<PaginatedList<Order>>
    {
        public string? CustomerId { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; }= 10;
    }

    public class GetOrdersWithPaginationQueryHandler: IRequestHandler<GetOrdersWithPaginationQuery, PaginatedList<Order>>
    {
        private readonly IMongoDbContext _context;

        public GetOrdersWithPaginationQueryHandler(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Order>> Handle(GetOrdersWithPaginationQuery request, CancellationToken cancellationToken)
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

            var filter = Builders<Order>.Filter.Eq("Customer._id", request.CustomerId);
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

            var data = aggregation.First()
                .Facets.First(x => x.Name == "dataFacet")
                .Output<Order>();

            return new PaginatedList<Order>(data, (int)count, request.PageNumber, request.PageSize);
        }
    }
}
