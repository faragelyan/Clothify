using Clothify.Application.DTOs.OrderItem;
using FluentValidation;

namespace Clothify.Application.FluentValidation.OrderItem
{
    public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemValidator()
        {
            RuleFor(oi => oi.OrderId).NotEmpty();
            RuleFor(oi => oi.ProductId).NotEmpty();
            RuleFor(oi => oi.Quantity).GreaterThan(0);
            RuleFor(oi => oi.Price).GreaterThanOrEqualTo(0);
        }
    }
}
