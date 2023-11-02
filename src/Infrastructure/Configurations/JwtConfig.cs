namespace CleanArchitectureTemplate.Infrastructure.Configurations;

public class JwtConfig
{
    public string Issuer { get; set; } = string.Empty;

    public string TokenKey { get; set; } = string.Empty;

    public string Audience { get => Issuer; }

    public int TokenExpirationInSeconds { get; set; }

    public int RefreshTokenExpirationInSeconds { get; set; }

    public string RefreshTokenCookieName { get; set; } = string.Empty;
}
