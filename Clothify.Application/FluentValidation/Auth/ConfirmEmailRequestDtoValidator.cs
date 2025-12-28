using Clothify.Application.DTOs.Auth;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Auth
{
    public class ConfirmEmailRequestDtoValidator : AbstractValidator<ConfirmEmailRequestDto>
    {
        public ConfirmEmailRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.VerificationCode)
                .NotEmpty().WithMessage("Verification code is required.")
                .Length(6, 6).WithMessage("Verification code must be 6 characters long.");
        }
    }
}
