using Common.Services;
using WeShare.Application.Common.Security;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;

[ServiceLevel(1)]
public class ServiceConnectionUserIdAuthorizationHandler : AuthorizationHandler<UserId, ServiceConnectionCommandOperation>
{
    public override ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, UserId entity, ServiceConnectionCommandOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            ServiceConnectionCommandOperation.Create 
                => ValueTask.FromResult(authenticatedUser == entity),
            _ => throw new InvalidOperationException(),
        };
    public override ValueTask<bool> HandleUnauthenticatedRequestAsync(UserId entity, ServiceConnectionCommandOperation operation,
        CancellationToken cancellationToken = default)
        => ValueTask.FromResult(false);
}
