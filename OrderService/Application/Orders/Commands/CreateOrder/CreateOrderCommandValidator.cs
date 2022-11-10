using FluentValidation;

namespace Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator: AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(o => o.TotalPrice)
                .GreaterThan(0)
                .WithMessage("TotalPrice must be greater than 0");
        }
    }
}
