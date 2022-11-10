using BLL.DTO.Requests;
using FluentValidation;

namespace BLL.Validation.Requests
{
    public class ProductRequestValidator : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidator()
        {
            RuleFor(product => product.Id)
                .NotEmpty()
                .WithMessage(product => $"{nameof(product.Id)} can't be empty.");

            RuleFor(product => product.Name)
                .NotEmpty()
                .WithMessage(product => $"{nameof(product.Name)} can't be empty.")
                .MaximumLength(50)
                .WithMessage(product => $"{nameof(product.Name)} should be less than 50 characters");

            RuleFor(product => product.Price)
                .NotEmpty()
                .WithMessage(product => $"{nameof(product.Price)} can't be empty.")
                .GreaterThan(0)
                .WithMessage(product => $"{nameof(product.Price)} should be more than 0.");

            RuleFor(product => product.ProductionTime)
                .NotEmpty()
                .WithMessage(product => $"{nameof(product.ProductionTime)} can't be empty.")
                .GreaterThanOrEqualTo(0)
                .WithMessage(product => $"{nameof(product.ProductionTime)} should be more or equeal to 0.");

            RuleFor(product => product.ShopId)
                .NotEmpty()
                .WithMessage(product => $"{nameof(product.ShopId)} can't be empty.");

        }
    }
}
