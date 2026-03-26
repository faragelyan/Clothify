using Clothify.Application.DTOs.CartItem;
using FluentValidation;

namespace Clothify.Application.FluentValidation.CartItem
{
    public class CartItemValidator : AbstractValidator<CartItemDto>
    {
        public CartItemValidator()
        {
            RuleFor(ci => ci.CartId)
                .NotEmpty();

            RuleFor(ci => ci.ProductId)
                .NotEmpty();

            RuleFor(ci => ci.Quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(999);
        }
    }
}
