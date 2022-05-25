using AutoMapper;
using BLL.DTO.Requests;
using BLL.DTO.Responses;
using BLL.Extensions;
using BLL.Interfaces.Services;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Pagination;
using DAL.Parameters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class UsersService: IUsersService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public UsersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            userManager = this.unitOfWork.UserManager;
        }

        public async Task<IEnumerable<UserResponse>> GetAsync()
        {
            var users = await userManager.Users.ToListAsync();
            return users?.Select(mapper.Map<User, UserResponse>);
        }

        public async Task<PagedList<UserResponse>> GetAsync(UserParameters parameters)
        {
            var users = await userManager.GetAsync(parameters);
            return users?.Map(mapper.Map<User, UserResponse>);
        }

        public async Task<UserResponse> GetByIdAsync(string id)
        {
            var user = await userManager.GetByIdAsync(id);
            return mapper.Map<User, UserResponse>(user);
        }

        public async Task UpdateAsync(UserRequest request)
        {
            var user = await userManager.GetByIdAsync(request.Id);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.DefaultAddressId = request.DefaultAddressId;

            await userManager.UpdateAsync(user);
            await unitOfWork.Commit();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await userManager.GetByIdAsync(id);
            await userManager.DeleteAsync(user);
            await unitOfWork.Commit();
        }
    }
}
