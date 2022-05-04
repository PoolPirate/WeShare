using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IOAuth2ExchangeService : IService
{
    Task<string?> RetrieveAccessTokenAsync(ServiceConnectionType type, string code, CancellationToken cancellationToken);
}

