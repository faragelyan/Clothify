using Clothify.Domain.Enums;

namespace Clothify.Domain.Entities
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public PaymentMethod PaymentMethod { get; set; } 
        public Guid OrderId { get; set; }
    }
}
