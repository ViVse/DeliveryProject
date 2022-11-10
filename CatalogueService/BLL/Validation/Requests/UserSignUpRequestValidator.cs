using BLL.DTO.Requests;
using FluentValidation;

namespace BLL.Validation.Requests
{
    public class UserSignUpRequestValidator: AbstractValidator<UserSignUpRequest>
    {
        public UserSignUpRequestValidator()
        {
            RuleFor(request => request.FirstName)
                .NotEmpty()
                .WithMessage("First name can't be empty")
                .MaximumLength(50)
                .WithMessage("First name should be less than 50 characters.");

            RuleFor(request => request.LastName)
                .NotEmpty()
                .WithMessage("Last name can't be empty")
                .MaximumLength(50)
                .WithMessage("Last name should be less than 50 characters.");

            RuleFor(request => request.Email)
                .EmailAddress()
                .WithMessage("Invalid email address");

            RuleFor(request => request.PhoneNumber)
                .Matches(@"^\d{10}$")
                .WithMessage("Invalid phone number (must be 10 digits long).");

            RuleFor(request => request.Password)
                .NotEmpty()
                .WithMessage("Password can't be empty")
                .Must(password => password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit))
                .WithMessage("Password should contain a digit, an uppercase and a lowercase letter");
        }
    }
}
