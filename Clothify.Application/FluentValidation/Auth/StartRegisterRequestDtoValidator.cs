using Clothify.Application.DTOs.Auth;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Auth
{
    public class StartRegisterRequestDtoValidator : AbstractValidator<StartRegisterRequestDto>
    {
        public StartRegisterRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
