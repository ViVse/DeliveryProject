using GraphQL.Data;
using GraphQL.Models;

namespace GraphQL.GraphQL.Shops
{
    public class ShopType : ObjectType<Shop>
    {
        protected override void Configure(IObjectTypeDescriptor<Shop> descriptor)
        {
            descriptor.Description("Represents any shop.");

            descriptor
                .Field(p => p.Id)
                .Description("Represents the unique ID for the shop.");

            descriptor
                .Field(p => p.Name)
                .Description("Represents the name of the shop.");


            descriptor
                .Field(p => p.Reviews)
                .ResolveWith<Resolvers>(p => p.GetReviews(default!, default!))
                .UseDbContext<AppDbContext>()
                .Description("This is the list of reviews on the shop.");
        }

        private class Resolvers
        {
            public IQueryable<Review> GetReviews([Parent] Shop shop, [ScopedService] AppDbContext context)
            {
                return context.Reviews.Where(p => p.ShopId == shop.Id);
            }
        }
    }
}
