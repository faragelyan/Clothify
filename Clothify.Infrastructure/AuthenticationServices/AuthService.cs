using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Auth;
using Clothify.Application.DTOs.User;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Clothify.Infrastructure.AuthenticationServices
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordHasher<AppUser> _passwordHasher = new();
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public AuthService(
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            IMapper mapper,
            IEmailService emailService,
            UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _emailService = emailService;
            _mapper = mapper;
            _userManager = userManager;
        }

        // -------------------------
        // Registration
        // -------------------------
        public async Task<Result<bool>> StartRegisterAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null) return Result<bool>.Fail("User already exists");

            var verificationCode = GenerateVerificationCode();

            var pending = await _unitOfWork.PendingVerifications
                .GetSingleEntityAsync(p => p.Email == email);

            if (pending == null)
            {
                pending = new PendingVerification { Email = email, IsConfirmed = false };
                await _unitOfWork.PendingVerifications.AddAsync(pending);
            }

            pending.VerificationCode = verificationCode;
            pending.Expiry = DateTime.UtcNow.AddMinutes(30);
            _unitOfWork.PendingVerifications.Update(pending);
            await _unitOfWork.CommitAsync();

            var emailBody = $@"
<div style='font-family:Arial, sans-serif; max-width:600px; margin:auto; background:#f8f9fa; padding:30px; border-radius:10px;'>
    <h2 style='text-align:center;'>Welcome to Clothify!</h2>
    <p>Use the verification code below to complete registration:</p>
    <div style='background:#3498db; padding:20px; text-align:center; border-radius:8px; margin:20px 0;'>
        <span style='color:#fff; font-size:24px;'>{verificationCode}</span>
    </div>
    <p>This code expires in 30 minutes.</p>
</div>";

            var sent = await _emailService.SendEmailAsync(email, "Clothify - Email Verification", emailBody, verificationCode);
            return sent ? Result<bool>.Ok(true) : Result<bool>.Fail("Failed to send verification email.");
        }

        public async Task<Result<bool>> ConfirmEmailAsync(ConfirmEmailRequestDto request)
        {
            var pending = await _unitOfWork.PendingVerifications
                .GetSingleEntityAsync(p => p.VerificationCode == request.VerificationCode);

            if (pending == null || pending.Email != request.Email)
                return Result<bool>.Fail("Invalid code or email mismatch.");
            if (pending.Expiry < DateTime.UtcNow)
                return Result<bool>.Fail("Verification code expired.");

            pending.IsConfirmed = true;
            _unitOfWork.PendingVerifications.Update(pending);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Ok(true);
        }

        public async Task<RegisterResponseDto> CompleteRegisterAsync(CreateUserDto request)
        {
            var pending = await _unitOfWork.PendingVerifications
                .GetSingleEntityAsync(p => p.Email == request.Email);

            if (pending == null)
                return new RegisterResponseDto { Message = "You must register first." };
            if (!pending.IsConfirmed)
                return new RegisterResponseDto { Message = "Email not confirmed." };

            var user = _mapper.Map<AppUser>(request);
            user.UserName = request.Email;
            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) return new RegisterResponseDto { Message = "Registration failed." };

            await _userManager.AddToRoleAsync(user, "User");

            _unitOfWork.PendingVerifications.Delete(pending);
            await _unitOfWork.CommitAsync();

            return new RegisterResponseDto { Message = "Registration succeeded!" };
        }

        // -------------------------
        // Login / Logout
        // -------------------------
        public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !user.EmailConfirmed)
                return Result<LoginResponseDto>.Fail("Invalid email or password.");

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
                return Result<LoginResponseDto>.Fail("Invalid email or password.");

            var loginResponse = await GenerateLoginResponseAsync(user);
            return Result<LoginResponseDto>.Ok(loginResponse);
        }

        public Task LogoutAsync(Guid userId) => throw new NotImplementedException();

        // -------------------------
        // Google Authentication
        // -------------------------
        public async Task<RegisterResponseDto> GoogleSignupAsync(GoogleSignupRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null) return new RegisterResponseDto { Message = "User already exists." };

            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
            var newUser = new AppUser
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                EmailConfirmed = true,
                UserName = payload.Email
            };

            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded) return new RegisterResponseDto { Message = "Registration failed." };

            await _userManager.AddToRoleAsync(newUser, "User");
            return new RegisterResponseDto { Message = "Registration succeeded!" };
        }

        public async Task<LoginResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null) throw new Exception("User not found, signup first.");

            return await GenerateLoginResponseAsync(user);
        }

        // -------------------------
        // Password Management
        // -------------------------
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var link = $"https://yourfrontend.com/reset-password?token={user.Id}";
            var emailBody = $"<p>Click <a href='{link}'>here</a> to reset your password. This link expires in 30 minutes.</p>";
            return await _emailService.SendEmailAsync(email, "Clothify - Password Reset", emailBody, link);
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var user = await _unitOfWork.AppUsers.FindAsync(request.Token);
            if (user == null) return false;

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            _unitOfWork.AppUsers.Update(user);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequestDto request)
        {
            var user = await _unitOfWork.AppUsers.FindAsync(request.Id);
            if (user == null) return Result<bool>.Fail("User not found.");

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.OldPassword);
            if (verify == PasswordVerificationResult.Failed)
                return Result<bool>.Fail("Current password invalid.");

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            _unitOfWork.AppUsers.Update(user);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Ok(true);
        }

        // -------------------------
        // Refresh Tokens
        // -------------------------
        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null) throw new Exception("Invalid token");

            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _unitOfWork.AppUsers.FindAsync(userId);
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

            var newAccessToken = _jwtService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = GenerateToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            _unitOfWork.AppUsers.Update(user);
            await _unitOfWork.CommitAsync();

            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(60)
            };
        }

        // -------------------------
        // Helpers
        // -------------------------
        private async Task<LoginResponseDto> GenerateLoginResponseAsync(AppUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

            var accessToken = _jwtService.GenerateAccessToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            _unitOfWork.AppUsers.Update(user);
            await _unitOfWork.CommitAsync();

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(60),
                Userinfo = _mapper.Map<UserDto>(user)
            };
        }

        private string GenerateToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string GenerateVerificationCode(int length = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var code = new char[length];
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            for (int i = 0; i < length; i++)
                code[i] = chars[bytes[i] % chars.Length];
            return new string(code);
        }
    }
}
