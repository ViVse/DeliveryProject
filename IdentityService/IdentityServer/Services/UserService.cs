using AutoMapper;
using IdentityServer.Data;
using IdentityServer.DTO.Request;
using IdentityServer.DTO.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper mapper;

        private readonly UserManager<UserModel> userManager;

        private readonly AspNetIdentityDbContext context;

        public UserService(IMapper mapper, UserManager<UserModel> userManager, AspNetIdentityDbContext context)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.context = context;
            /*context.Database.EnsureCreated();*/
        }

        public async Task SignUpAsync(SignUpRequest request)
        {
            var user = mapper.Map<SignUpRequest, UserModel>(request);
            user.UserName = user.Email;
            /*var signUpResult = await userManager.CreateAsync(user, request.Password);

            if (!signUpResult.Succeeded)
            {
                string errors = string.Join("\n",
                    signUpResult.Errors.Select(err => err.ToString()));

                throw new ArgumentException(errors);
            }*/

            var result = userManager.CreateAsync(user, request.Password).Result;
            if (!result.Succeeded)
            {
               throw new Exception(result.Errors.First().Description);
            }

            result = userManager.AddClaimsAsync( user,
                new Claim[]
                {
                    new Claim("role", "user")
                }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserResponse>> GetAsync()
        {
            var users = await userManager.Users.ToListAsync();
            return users?.Select(mapper.Map<UserModel, UserResponse>);
        }

        public async Task<UserResponse> GetByIdAsync(string id)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<UserModel, UserResponse>(user);
        }

        public async Task UpdateAsync(UserRequest request)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(x => x.Id == request.Id);

            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            await userManager.UpdateAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(x => x.Id == id);
            await userManager.DeleteAsync(user);
            await context.SaveChangesAsync();
        }
    }
}
