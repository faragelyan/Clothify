namespace Clothify.Application.DTOs.Auth
{
    public class ChangePasswordRequestDto
    {
        public Guid Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
