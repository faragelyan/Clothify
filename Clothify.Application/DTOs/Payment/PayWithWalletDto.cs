namespace Clothify.Application.DTOs.Payment
{
    public class PayWithWalletDto
    {
        public Guid OrderId { get; set; }
        public string PhoneNumber { get; set; } = null!;
    }
}
