using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CurrencyConversion.Api.Authorization;
using CurrencyConversion.Api.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CurrencyConversion.Api.Controllers.V1;

/// <summary>
/// Token endpoint for obtaining JWT (e.g. for Swagger/testing).
/// In production, replace with your identity provider or login flow.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly JwtOptions _jwtOptions;

    public AuthController(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    /// <summary>
    /// Issue a JWT for the given client and role (for testing). Use the returned token in Authorization: Bearer {token}.
    /// </summary>
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetToken([FromBody] TokenRequest request)
    {
        var role = string.IsNullOrWhiteSpace(request?.Role)
            ? Roles.User
            : request.Role.Equals(Roles.Admin, StringComparison.OrdinalIgnoreCase)
                ? Roles.Admin
                : Roles.User;

        var clientId = string.IsNullOrWhiteSpace(request?.ClientId) ? "test-client" : request.ClientId.Trim();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, clientId),
            new("sub", clientId),
            new(ClaimTypes.Role, role)
        };

        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new TokenResponse(tokenString, expires, "No Authorize do Swagger, cole apenas o valor de 'token' (não digite Bearer na frente)."));
    }
}

/// <summary>Request body for token endpoint.</summary>
public record TokenRequest(string? ClientId, string? Role);

/// <summary>Response with JWT and expiry.</summary>
public record TokenResponse(string Token, DateTime ExpiresAt, string? UsageHint = null);
