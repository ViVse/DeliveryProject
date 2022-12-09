using Cart.API.Entities;

namespace Cart.API.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<ShoppingCart> GetBasket(string userId);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(string userId);
    }
}
