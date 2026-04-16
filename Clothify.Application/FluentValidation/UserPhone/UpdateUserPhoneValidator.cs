using Clothify.Application.DTOs.UserPhone;
using FluentValidation;

namespace Clothify.Application.FluentValidation.UserPhone
{
    public class UpdateUserPhoneValidator : AbstractValidator<UpdateUserPhoneDto>
    {
        public UpdateUserPhoneValidator()
        {
            RuleFor(p => p.PhoneId).NotEmpty();
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
