using FluentValidation;

namespace Application.Orders.Queries.GetOrdersWithPagination
{
    public class GetOrdersWithPaginationQueryValidator: AbstractValidator<GetOrdersWithPaginationQuery>
    {
        public GetOrdersWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize must be at least greater than or equal to 1.");
        }
    }
}
