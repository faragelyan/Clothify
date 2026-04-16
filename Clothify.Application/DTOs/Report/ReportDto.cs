using System;
using Clothify.Domain.Enums;

namespace Clothify.Application.DTOs.Report
{
    public class ReportDto
    {
        public Guid ReportId { get; set; }
        public ReportType ReportType { get; set; }
        public DateTime GeneratedAt { get; set; }
        public Guid UserId { get; set; }
    }
}
