using Common.Services;
using WeShare.Application.Services;
using WeShare.Domain.Entities;
using WeShare.Infrastructure.Services.OAuth2;

namespace WeShare.Infrastructure.Services;
public class OAuth2ExchangeService : Singleton, IOAuth2ExchangeService
{
    [Inject]
    private readonly DiscordOAuth2Handler DiscordOAuth2Handler;

    public Task<string?> RetrieveAccessTokenAsync(ServiceConnectionType type, string code, CancellationToken cancellationToken)
        => type switch
        {
            ServiceConnectionType.None => throw new InvalidOperationException(),
            ServiceConnectionType.Discord => DiscordOAuth2Handler.GetAccessTokenAsync(code, cancellationToken),

            _ => throw new InvalidOperationException(),
        };
}

