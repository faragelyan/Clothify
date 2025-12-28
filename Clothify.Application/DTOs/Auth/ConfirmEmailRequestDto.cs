namespace Clothify.Application.DTOs.Auth
{
    public class ConfirmEmailRequestDto
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}
