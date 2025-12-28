namespace Clothify.Application.DTOs.Auth
{
    public class GoogleLoginRequestDto
    {
        public string IdToken { get; set; } = default!;  // Google returns this token
    }
}
