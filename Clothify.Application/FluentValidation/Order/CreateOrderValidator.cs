using Clothify.Application.DTOs.Order;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Order
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
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
