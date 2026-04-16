using Clothify.Application.DTOs.OrderItem;
using FluentValidation;

namespace Clothify.Application.FluentValidation.OrderItem
{
    public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItemDto>
    {
        public UpdateOrderItemValidator()
        {
            RuleFor(oi => oi.OrderId).NotEmpty();
            RuleFor(oi => oi.ProductId).NotEmpty();
            RuleFor(oi => oi.Quantity).GreaterThan(0);
            RuleFor(oi => oi.Price).GreaterThanOrEqualTo(0);
        }
    }
}
