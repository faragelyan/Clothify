using Clothify.Domain.Enums;

namespace Clothify.Application.DTOs.Payment
{
    public class UpdatePaymentDto
    {
        public Guid PaymentId { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
