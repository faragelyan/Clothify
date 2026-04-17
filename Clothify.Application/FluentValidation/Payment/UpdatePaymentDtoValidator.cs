using FluentValidation;
using Clothify.Application.DTOs.Payment;

namespace Clothify.Application.FluentValidation.Payment
{
    public class UpdatePaymentDtoValidator : AbstractValidator<UpdatePaymentDto>
    {
        public UpdatePaymentDtoValidator()
        {
            RuleFor(x => x.PaymentId)
                .NotEmpty().WithMessage("Payment ID is required");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid payment status");
        }
    }
}
