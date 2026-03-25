using Clothify.Application.DTOs.Address;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Address
{
    public class AddressValidator : AbstractValidator<AddressDto>
    {
        public AddressValidator()
        {
            RuleFor(a => a.FullAddress)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(a => a.AddressType)
                .IsInEnum();
        }
    }
}
