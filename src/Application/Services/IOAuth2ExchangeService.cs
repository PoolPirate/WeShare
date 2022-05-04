using Common.Services;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IOAuth2ExchangeService : IService
{
    Task<OAuth2TokenResponse?> ExchangeCodeForTokens(ServiceConnectionType type, string code, CancellationToken cancellationToken);
}

