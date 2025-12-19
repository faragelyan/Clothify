namespace Clothify.Domain.Entities
{
    public class UserPhone
    {
        public Guid PhoneId { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Type { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
