using ABCCommerce.Server.Controllers;
using ABCCommerceDataAccess.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ABCCommerce.Server.Services;

public class TokenService
{
    public IConfiguration Configuration { get; }
    public TokenService(IConfiguration configuration)
    {
        Configuration = configuration;
    }


    public TokenResponse CreateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Configuration["JwtSettings:Key"]!);

        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("userid", user.Id.ToString()),
            new(ClaimTypes.Role, user.Roles),
            new(JwtRegisteredClaimNames.Email, user.Email),
        };
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(120);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            SigningCredentials = credentials
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenText = tokenHandler.WriteToken(token);

        var refreshClaims = new List<Claim>
        {
            new("userid", user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        var refreshTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(refreshClaims),
            Expires = DateTime.UtcNow.AddDays(100),
            SigningCredentials = credentials
        };
        var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);
        var refreshTokenText = tokenHandler.WriteToken(refreshToken);

        return new TokenResponse(tokenText, refreshTokenText, "Bearer", expiresAt);
    }

    public ClaimsPrincipal ValidateToken(string tokenText, out SecurityToken token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Configuration["JwtSettings:Key"]!);

        var securityKey = new SymmetricSecurityKey(key);
        return tokenHandler.ValidateToken(tokenText, new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
        },
        out token);
    }
}