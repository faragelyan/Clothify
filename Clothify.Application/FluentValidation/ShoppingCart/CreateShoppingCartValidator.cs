using Clothify.Application.DTOs.ShoppingCart;
using FluentValidation;

namespace Clothify.Application.FluentValidation.ShoppingCart
{
    public class CreateShoppingCartValidator : AbstractValidator<CreateShoppingCartDto>
    {
        public CreateShoppingCartValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
