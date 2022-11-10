using GraphQL.Data;
using GraphQL.GraphQL.Reviews;
using GraphQL.GraphQL.Shops;
using GraphQL.GraphQL.Users;
using GraphQL.Models;
using HotChocolate.Subscriptions;

namespace GraphQL.GraphQL
{
    public class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddUserPayload> AddUserAsync(
            AddUserInput input,
            [ScopedService] AppDbContext context,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken
            )
        {
            var user = new User
            {
                FirstName = input.FirstName,
                LastName = input.LastName
            };

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            await eventSender.SendAsync(nameof(Subscription.OnUserAdded), user, cancellationToken);

            return new AddUserPayload(user);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddShopPayload> AddShopAsync(
            AddShopInput input,
            [ScopedService] AppDbContext context,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken
            )
        {
            var shop = new Shop
            {
                Name = input.name
            };

            context.Shops.Add(shop);
            await context.SaveChangesAsync(cancellationToken);

            return new AddShopPayload(shop);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddReviewPayload> AddReviewAsync(AddReviewInput input, [ScopedService] AppDbContext context)
        {
            var review = new Review
            {
                Text = input.Text,
                UserId = input.UserId,
                ShopId = input.ShopId
            };

            context.Reviews.Add(review);
            await context.SaveChangesAsync();
            return new AddReviewPayload(review);
        }
    }
}
