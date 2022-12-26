using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderModel> GetOrder(string orderId);
    }
}
