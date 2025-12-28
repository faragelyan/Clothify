namespace Clothify.Application.DTOs.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime LastActiveAt { get; set; }
    }
}
