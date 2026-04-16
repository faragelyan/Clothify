using Clothify.Application.DTOs.Order;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Order
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderValidator()
        {
            RuleFor(o => o.OrderId)
                .NotEmpty()
                .WithMessage("OrderId is required.");

            RuleFor(o => o.TotalAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total amount must be greater than or equal to zero.");

            RuleFor(o => o.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(o => o.AddressId)
                .NotEmpty()
                .WithMessage("AddressId is required.");
        }
    }
}
