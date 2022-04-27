using Common.Services;

namespace WeShare.Application.Services;
public interface IAuthorizer : IService
{
    public ValueTask EnsureAuthorizationAsync(object entity, Enum operation, CancellationToken cancellationToken);
}

