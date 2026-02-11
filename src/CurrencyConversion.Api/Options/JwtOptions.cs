namespace CurrencyConversion.Api.Options;

/// <summary>
/// JWT bearer authentication configuration.
/// </summary>
public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    /// <summary>Issuer (iss claim).</summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>Audience (aud claim).</summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>Secret key used to sign tokens. Must be long enough for the signing algorithm (e.g. 32+ chars for HS256).</summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>Token lifetime in minutes.</summary>
    public int ExpirationMinutes { get; set; } = 60;
}
