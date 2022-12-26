using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Interfaces;

namespace Shopping.Aggregator.Services
{
    public class UserService: IUserService
    {
        private readonly HttpClient _client;

        public UserService(HttpClient client)
        {
            _client = client;
        }

        public async Task<UserModel> GetUser(string userId)
        {
            var response = await _client.GetAsync($"/api/Users/{userId}");
            return await response.ReadContentAs<UserModel>();
        }
    }
}
