using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Common.Security;

[ServiceLevel(0)]
public abstract class AuthorizationHandler<TEntity, TOperation> : Scoped where TOperation : Enum
{
    public abstract ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, TEntity entity, TOperation operation, CancellationToken cancellationToken = default);
    public abstract ValueTask<bool> HandleUnauthenticatedRequestAsync(TEntity entity, TOperation operation, CancellationToken cancellationToken = default);
}
