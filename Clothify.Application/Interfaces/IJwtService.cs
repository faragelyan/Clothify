using System.Security.Claims;
namespace Clothify.Application.Interfaces
{
    public interface IJwtService
    {
        public string GenerateAccessToken(IEnumerable<Claim> claims);
        public string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
