using FluentValidation;

namespace Application.Orders.Commands.UpdateOrder
{
    public class UodateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UodateOrderCommandValidator()
        {
            RuleFor(o => o.TotalPrice)
                .GreaterThan(0)
                .WithMessage("TotalPrice must be greater than 0");
        }
    }
}
