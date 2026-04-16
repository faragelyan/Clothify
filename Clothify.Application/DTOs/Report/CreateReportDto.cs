using System;
using Clothify.Domain.Enums;

namespace Clothify.Application.DTOs.Report
{
    public class CreateReportDto
    {
        public ReportType ReportType { get; set; }
        public Guid UserId { get; set; }
    }
}
