using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using User.API.Data;
using User.API.DTO.Request;
using User.API.DTO.Response;
using User.API.Exceptions;
using User.API.Interfaces;

namespace User.API.Services
{
    public class UserService: IUserService
    {
        private readonly IMapper mapper;

        private readonly IJwtSecurityTokenFactory tokenFactory;

        private readonly UserManager<UserModel> userManager;

        private readonly Context context;

        public UserService(IMapper mapper, IJwtSecurityTokenFactory tokenFactory, UserManager<UserModel> userManager, Context context)
        {
            this.mapper = mapper;
            this.tokenFactory = tokenFactory;
            this.userManager = userManager;
            this.context = context;
            context.Database.EnsureCreated();
        }

        private static string SerializeToken(JwtSecurityToken jwtToken) =>
            new JwtSecurityTokenHandler().WriteToken(jwtToken);

        public async Task<JwtResponse> SignInAsync(SignInRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.UserName)
                ?? throw new EntityNotFoundException("Incorrect email or password");

            if (!await userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new EntityNotFoundException("Incorrect email or password");
            }

            var jwtToken = tokenFactory.BuildToken(user);
            return new JwtResponse() { Token = SerializeToken(jwtToken) };
        }

        public async Task<JwtResponse> SignUpAsync(SignUpRequest request)
        {
            var user = mapper.Map<SignUpRequest, UserModel>(request);
            user.UserName = user.Email;
            var signUpResult = await userManager.CreateAsync(user, request.Password);

            if (!signUpResult.Succeeded)
            {
                string errors = string.Join("\n",
                    signUpResult.Errors.Select(err => err.ToString()));

                throw new ArgumentException(errors);
            }

            await context.SaveChangesAsync();
            var jwtToken = tokenFactory.BuildToken(user);
            return new JwtResponse() { Token = SerializeToken(jwtToken) };
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
