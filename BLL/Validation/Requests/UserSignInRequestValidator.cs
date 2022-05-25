using BLL.DTO.Requests;
using FluentValidation;

namespace BLL.Validation.Requests
{
    public class UserSignInRequestValidator: AbstractValidator<UserSignInRequest>
    {
        public UserSignInRequestValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty()
                .WithMessage("Email can't be empty")
                .EmailAddress()
                .WithMessage("Not a valid email");

            RuleFor(request => request.Password)
                .NotEmpty()
                .WithMessage("Password can't be empty");
        }
    }
}
