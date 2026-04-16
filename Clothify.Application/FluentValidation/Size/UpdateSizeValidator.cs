using Clothify.Application.DTOs.Size;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Size
{
    public class UpdateSizeValidator : AbstractValidator<UpdateSizeDto>
    {
        public UpdateSizeValidator()
        {
            RuleFor(s => s.SizeId).NotEmpty();
            RuleFor(s => s.Name)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
