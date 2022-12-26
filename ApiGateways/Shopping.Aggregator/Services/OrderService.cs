using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Interfaces;

namespace Shopping.Aggregator.Services
{
    public class OrderService: IOrderService
    {
        private readonly HttpClient _client;

        public OrderService(HttpClient client)
        {
            _client = client;
        }

        public async Task<OrderModel> GetOrder(string orderId)
        {
            var response = await _client.GetAsync($"/api/Orders/{orderId}");
            return await response.ReadContentAs<OrderModel>();
        }
    }
}
