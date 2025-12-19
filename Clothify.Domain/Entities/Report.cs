namespace Clothify.Domain.Entities
{
    public class Report
    {
        public Guid ReportID { get; set; }
        public string ReportType { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
