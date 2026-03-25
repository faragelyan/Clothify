using Clothify.Application.DTOs.Address;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Address
{
    public class UpdateAddressValidator : AbstractValidator<UpdateAddressDto>
    {
        public UpdateAddressValidator()
        {
            RuleFor(a => a.AddressId)
                .NotEmpty();

            RuleFor(a => a.FullAddress)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(a => a.AddressType)
                .IsInEnum();
        }
    }
}
