using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Interfaces;

namespace Shopping.Aggregator.Services
{
    public class ShopService : IShopService
    {
        private readonly HttpClient _client;

        public ShopService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ShopModel> GetShop(int shopId)
        {
            var response = await _client.GetAsync($"/api/Shops/{shopId}");
            return await response.ReadContentAs<ShopModel>();
        }
    }
}
