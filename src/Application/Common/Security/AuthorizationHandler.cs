using Common.Services;

namespace WeShare.Application.Common.Security;

[ServiceLevel(0)]
public abstract class AuthorizationHandler<TEntity, TOperation> : Scoped where TOperation : Enum
{
    public abstract ValueTask<bool> HandleAuthorizationRequestAsync(TEntity entity, TOperation operation, CancellationToken cancellationToken = default);
}
