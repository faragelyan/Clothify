using Clothify.Domain.Enums;

namespace Clothify.Domain.Entities
{
    public class Report
    {
        public Guid ReportId { get; set; }
        public ReportType ReportType { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
    }
}
