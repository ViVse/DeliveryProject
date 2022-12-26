using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services.Interfaces
{
    public interface IShopService
    {
        Task<ShopModel> GetShop(int shopId);
    }
}
