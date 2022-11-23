using Cart.API.Entities;
using Cart.API.Extensions;
using Cart.API.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Cart.API.Repositories
{
    public class CartRepository: ICartRepository
    {
        private readonly IDistributedCache _redisCache;

        public CartRepository(IDistributedCache cache)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            return await _redisCache.GetRecordAsync<ShoppingCart>(userName);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetRecordAsync<ShoppingCart>(basket.UserName, basket, TimeSpan.FromDays(7), TimeSpan.FromDays(2));

            return await GetBasket(basket.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }
    }
}
