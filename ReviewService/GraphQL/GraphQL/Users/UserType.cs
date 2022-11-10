using GraphQL.Data;
using GraphQL.Models;

namespace GraphQL.GraphQL.Users
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.Description("Represents any user.");

            descriptor
                .Field(p => p.Id)
                .Description("Represents the unique ID for the user.");

            descriptor
                .Field(p => p.FirstName)
                .Description("Represents the first name for the user.");

            descriptor
                .Field(p => p.LastName)
                .Description("Represents the last name for the user.");

            descriptor
                .Field(p => p.Reviews)
                .ResolveWith<Resolvers>(p => p.GetReviews(default!, default!))
                .UseDbContext<AppDbContext>()
                .Description("This is the list of reviews written by the user.");
        }

        private class Resolvers
        {
            public IQueryable<Review> GetReviews([Parent] User user, [ScopedService] AppDbContext context)
            {
                return context.Reviews.Where(p => p.UserId == user.Id);
            }
        }
    }
}
