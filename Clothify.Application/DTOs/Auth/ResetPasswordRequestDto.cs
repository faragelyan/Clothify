namespace Clothify.Application.DTOs.Auth
{
    public class ResetPasswordRequestDto
    {
        public Guid Token { get; set; }
        public string NewPassword { get; set; }
    }
}
