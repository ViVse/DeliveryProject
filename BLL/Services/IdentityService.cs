using AutoMapper;
using BLL.DTO.Requests;
using BLL.DTO.Responses;
using BLL.Interfaces;
using BLL.Interfaces.Services;
using DAL.Entities;
using DAL.Exceptions;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace BLL.Services
{
    public class IdentityService: IIdentityService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        private readonly IJwtTokenFactory tokenFactory;

        private readonly UserManager<User> userManager;

        public IdentityService(IUnitOfWork unitOfWork, IMapper mapper, IJwtTokenFactory tokenFactory)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.tokenFactory = tokenFactory;
            userManager = this.unitOfWork.UserManager;
        }

        private static string SerializeToken(JwtSecurityToken jwtToken) =>
            new JwtSecurityTokenHandler().WriteToken(jwtToken);

        public async Task<JwtResponse> SignInAsync(UserSignInRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email)
                ?? throw new EntityNotFoundException("Incorrect email or password");

            if(!await userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new EntityNotFoundException("Incorrect email or password");
            }

            var jwtToken = tokenFactory.BuildToken(user);
            return new JwtResponse() { Token = SerializeToken(jwtToken) };
        }

        public async Task<JwtResponse> SignUpAsync(UserSignUpRequest request)
        {
            var user = mapper.Map<UserSignUpRequest, User>(request);
            user.UserName = user.Email;
            user.Avatar = "default";
            var signUpResult = await userManager.CreateAsync(user, request.Password);

            if (!signUpResult.Succeeded)
            {
                string errors = string.Join("\n",
                    signUpResult.Errors.Select(err => err.ToString()));

                throw new ArgumentException(errors);
            }

            await unitOfWork.Commit();

            var jwtToken = tokenFactory.BuildToken(user);
            return new JwtResponse() { Token = SerializeToken(jwtToken) };
        }
    }
}
