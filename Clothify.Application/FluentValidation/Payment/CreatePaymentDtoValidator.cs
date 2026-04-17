using FluentValidation;
using Clothify.Application.DTOs.Payment;

namespace Clothify.Application.FluentValidation.Payment
{
    public class CreatePaymentDtoValidator : AbstractValidator<CreatePaymentDto>
    {
        public CreatePaymentDtoValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required");

            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order ID is required");
        }
    }
}
