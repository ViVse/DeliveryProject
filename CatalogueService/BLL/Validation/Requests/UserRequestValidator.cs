using BLL.DTO.Requests;
using FluentValidation;

namespace BLL.Validation.Requests
{
    public class UserRequestValidator: AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(request => request.Email)
                .EmailAddress()
                .WithMessage("Wrong email address.");

            RuleFor(request => request.FirstName)
                .NotEmpty()
                .WithMessage(request => $"{nameof(request.FirstName)} cannot be empty.")
                .MaximumLength(50)
                .WithMessage(request => $"{nameof(request.FirstName)} should be less than 50 characters.");

            RuleFor(request => request.LastName)
                .NotEmpty()
                .WithMessage(request => $"{nameof(request.LastName)} cannot be empty.")
                .MaximumLength(50)
                .WithMessage(request => $"{nameof(request.LastName)} should be less than 50 characters.");

            RuleFor(request => request.PhoneNumber)
                .Matches(@"^\d{10}$")
                .WithMessage("Invalid phone number (must be 10 digits long).");
        }
    }
}
