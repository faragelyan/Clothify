namespace Clothify.Application.DTOs.User
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
