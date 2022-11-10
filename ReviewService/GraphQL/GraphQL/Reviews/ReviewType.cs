using GraphQL.Data;
using GraphQL.Models;

namespace GraphQL.GraphQL.Reviews
{
    public class ReviewType : ObjectType<Review>
    {
        protected override void Configure(IObjectTypeDescriptor<Review> descriptor)
        {
            descriptor.Description("Represents user review for the shop");

            descriptor
                .Field(p => p.Id)
                .Description("Represents the unique ID for the review.");

            descriptor
                .Field(p => p.Text)
                .Description("Represents the ftext of the review.");

            descriptor
                .Field(p => p.UserId)
                .Description("Represents id of user who wrote the review.");

            descriptor
                .Field(p => p.ShopId)
                .Description("Represents id of shop that was reviewed.");

            descriptor
                .Field(p => p.User)
                .ResolveWith<Resolvers>(c => c.GetUser(default!, default!))
                .UseDbContext<AppDbContext>()
                .Description("This is the user to which the review belongs.");

            descriptor
                .Field(p => p.Shop)
                .ResolveWith<Resolvers>(c => c.GetShop(default!, default!))
                .UseDbContext<AppDbContext>()
                .Description("This is the shop to which the review belongs.");
        }

        private class Resolvers
        {
            public User GetUser([Parent] Review review, [ScopedService] AppDbContext context)
            {
                return context.Users.FirstOrDefault(p => p.Id == review.UserId);
            }

            public Shop GetShop([Parent] Review review, [ScopedService] AppDbContext context)
            {
                return context.Shops.FirstOrDefault(p => p.Id == review.ShopId);
            }
        }
    }
}
