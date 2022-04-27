using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WeShare.WebAPI.Services.Configuration;

public class JwtBearerOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly TokenValidationParameters TokenValidationParameters;

    public JwtBearerOptionsConfiguration(TokenValidationParameters tokenValidationParameters)
    {
        TokenValidationParameters = tokenValidationParameters;
    }

    public void Configure(string name, JwtBearerOptions options) => options.TokenValidationParameters = TokenValidationParameters;

    public void Configure(JwtBearerOptions options) => options.TokenValidationParameters = TokenValidationParameters;
}
