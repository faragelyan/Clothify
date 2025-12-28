namespace Clothify.Application.DTOs.User
{
    public class CreateUserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }
    }
}
