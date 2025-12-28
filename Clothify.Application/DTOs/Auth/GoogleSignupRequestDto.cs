namespace Clothify.Application.DTOs.Auth
{
    public class GoogleSignupRequestDto
    {
        public string Email { get; set; } = default!;
        public string IdToken { get; set; } = default!; // token from Google for verification
    }
}
