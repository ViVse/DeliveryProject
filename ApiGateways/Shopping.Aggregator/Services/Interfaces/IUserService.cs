using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> GetUser(string userId);
    }
}
