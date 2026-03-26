using Clothify.Application.DTOs.CartItem;
using FluentValidation;

namespace Clothify.Application.FluentValidation.CartItem
{
    public class UpdateCartItemValidator : AbstractValidator<UpdateCartItemDto>
    {
        public UpdateCartItemValidator()
        {
            RuleFor(ci => ci.ProductId)
                .NotEmpty();

            RuleFor(ci => ci.Quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(999);
        }
    }
}
