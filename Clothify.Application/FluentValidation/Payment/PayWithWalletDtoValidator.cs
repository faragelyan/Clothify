using FluentValidation;
using Clothify.Application.DTOs.Payment;

namespace Clothify.Application.FluentValidation.Payment
{
    public class PayWithWalletDtoValidator : AbstractValidator<PayWithWalletDto>
    {
        public PayWithWalletDtoValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order ID is required");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^01[0125][0-9]{8}$").WithMessage("Invalid Egyptian mobile number format");
        }
    }
}
