using Clothify.Application.DTOs.CartItem;
using FluentValidation;

namespace Clothify.Application.FluentValidation.CartItem
{
    public class CreateCartItemValidator : AbstractValidator<CreateCartItemDto>
    {
        public CreateCartItemValidator()
        {
            RuleFor(ci => ci.ProductId)
                .NotEmpty();

            RuleFor(ci => ci.Quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(999);
        }
    }
}
