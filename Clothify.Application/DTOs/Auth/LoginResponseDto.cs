using Clothify.Application.DTOs.User;
namespace Clothify.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime Expiration { get; set; }
        public UserDto Userinfo { get; set; }
    }
}
