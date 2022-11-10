using GraphQL.Data;
using GraphQL.Models;

namespace GraphQL.GraphQL
{
    public class Query
    {
        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<User> GetUsers([ScopedService] AppDbContext context)
        {
            return context.Users;
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Shop> GetShops([ScopedService] AppDbContext context)
        {
            return context.Shops;
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Review> GetReviews([ScopedService] AppDbContext context)
        {
            return context.Reviews;
        }
    }
}
