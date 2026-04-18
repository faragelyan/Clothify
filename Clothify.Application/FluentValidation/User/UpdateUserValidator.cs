using Clothify.Application.DTOs.User;
using FluentValidation;

namespace Clothify.Application.FluentValidation.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("First name must contain only letters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("Last name must contain only letters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .Must(BeAValidAge).WithMessage("User must be at least 13 years old.");
        }

        private bool BeAValidAge(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age)) age--;
            return age >= 13;
        }
    }
}
