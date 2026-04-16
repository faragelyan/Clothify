using Clothify.Application.DTOs.UserPhone;
using FluentValidation;

namespace Clothify.Application.FluentValidation.UserPhone
{
    public class CreateUserPhoneValidator : AbstractValidator<CreateUserPhoneDto>
    {
        public CreateUserPhoneValidator()
        {
            RuleFor(p => p.UserId).NotEmpty();
            RuleFor(p => p.PhoneNumber)
                .NotEmpty()
                .MaximumLength(20);
            RuleFor(p => p.Type)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
