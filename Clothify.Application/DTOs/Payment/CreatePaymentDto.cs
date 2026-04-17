using Clothify.Domain.Enums;

namespace Clothify.Application.DTOs.Payment
{
    public class CreatePaymentDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; }
        public Guid OrderId { get; set; }
    }
}
