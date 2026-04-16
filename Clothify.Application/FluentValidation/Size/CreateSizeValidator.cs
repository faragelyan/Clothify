using Clothify.Application.DTOs.Size;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Size
{
    public class CreateSizeValidator : AbstractValidator<CreateSizeDto>
    {
        public CreateSizeValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
