using Common.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeShare.Application.Services;
using WeShare.Domain.Entities;
using WeShare.Infrastructure.Options;

namespace WeShare.Infrastructure.Services;
public class JwtService : Singleton, IJwtService
{
    [Inject]
    private readonly JwtOptions JwtOptions;
    [Inject]
    private readonly JwtSecurityTokenHandler JwtSecurityTokenHandler;

    private byte[] JwtKeyBytes;

    protected override ValueTask InitializeAsync()
    {
        JwtKeyBytes = Encoding.UTF8.GetBytes(JwtOptions.JwtKey);
        return base.InitializeAsync();
    }

    public string GenerateUserLoginJWT(UserId userId, out int expiresInSeconds)
    {
        var claims = new List<Claim>(1)
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        };

        expiresInSeconds = JwtOptions.JwtExpirationSeconds;
        return GenerateJwt(claims);
    }

    private string GenerateJwt(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(JwtKeyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha384);

        var token = new JwtSecurityToken(JwtOptions.JwtIssuer,
          JwtOptions.JwtIssuer,
          claims,
          expires: DateTime.Now + TimeSpan.FromSeconds(JwtOptions.JwtExpirationSeconds),
          signingCredentials: credentials);

        return JwtSecurityTokenHandler.WriteToken(token);
    }
}
