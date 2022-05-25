using DAL.Entities;
using DAL.Exceptions;
using DAL.Pagination;
using DAL.Parameters;
using Microsoft.AspNetCore.Identity;

namespace BLL.Extensions
{
    public static class UserManagerExtension
    {
        
        public static async Task<PagedList<User>> GetAsync(
            this UserManager<User> userManager,
            UserParameters parameters)
        {
            var source = userManager.Users;

            return await PagedList<User>.ToPagedListAsync(source, parameters.PageNumber, parameters.PageSize);
        }

        public static async Task<User> GetByIdAsync(
            this UserManager<User> userManager,
            string id)
        {
            var user = await userManager.FindByIdAsync(id);
            return user ?? throw new EntityNotFoundException(GetUserNotFoundErrorMessage(id));
        }

        private static string GetUserNotFoundErrorMessage(string id) =>
            $"{nameof(User)} with id {id} not found.";
    }
}
