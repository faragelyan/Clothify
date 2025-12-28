namespace Clothify.Domain.Entities
{
    public class PendingVerification
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string VerificationCode { get; set; } = null!;
        public DateTime Expiry { get; set; }
        public bool IsConfirmed { get; set; } = false;
    }
}
