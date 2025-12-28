namespace Clothify.Infrastructure.AuthenticationServices
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string LifeTime { get; set; }
        public string SigningKey { get; set; }
    }
}
