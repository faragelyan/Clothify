using Clothify.Application.DTOs.Report;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Report
{
    public class CreateReportValidator : AbstractValidator<CreateReportDto>
    {
        public CreateReportValidator()
        {
            RuleFor(r => r.UserId).NotEmpty();
            RuleFor(r => r.ReportType).IsInEnum();
        }
    }
}
