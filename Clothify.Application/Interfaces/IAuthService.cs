using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Auth;
using Clothify.Application.DTOs.User;
namespace Clothify.Application.Interfaces
{
    public interface IAuthService
    {
        // -------------------------
        // Register & Login
        // -------------------------
        Task<Result<bool>> StartRegisterAsync(string emaiL);
        Task<Result<bool>> ConfirmEmailAsync(ConfirmEmailRequestDto confirmEmailRequestDto);
        Task<RegisterResponseDto> CompleteRegisterAsync(CreateUserDto request);
        Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request);
        Task LogoutAsync(Guid userId);



        // -------------------------
        // Google
        // -------------------------
        Task<RegisterResponseDto> GoogleSignupAsync(GoogleSignupRequestDto request);
        Task<LoginResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request);
        // -------------------------
        // Password Management
        // -------------------------
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request);
        Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequestDto request);

        // -------------------------
        // Refresh Tokens
        // -------------------------
        Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}
