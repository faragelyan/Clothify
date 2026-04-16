using Clothify.Application.DTOs.ShoppingCart;
using FluentValidation;

namespace Clothify.Application.FluentValidation.ShoppingCart
{
    public class UpdateShoppingCartValidator : AbstractValidator<UpdateShoppingCartDto>
    {
        public UpdateShoppingCartValidator()
        {
            RuleFor(x => x.CartId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);
        }
    }
}
