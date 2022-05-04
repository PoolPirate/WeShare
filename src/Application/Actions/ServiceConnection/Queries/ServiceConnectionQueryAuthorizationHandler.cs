using Common.Services;
using WeShare.Application.Common.Security;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;

[ServiceLevel(1)]
public class ServiceConnectionQueryAuthorizationHandler : AuthorizationHandler<UserId, ServiceConnectionQueryOperation>
{
    public override ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, UserId entity, ServiceConnectionQueryOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            ServiceConnectionQueryOperation.ReadSnippets 
                => ValueTask.FromResult(authenticatedUser == entity),
            _ => throw new InvalidOperationException(),
        };

    public override ValueTask<bool> HandleUnauthenticatedRequestAsync(UserId entity, ServiceConnectionQueryOperation operation, 
        CancellationToken cancellationToken = default) 
        => ValueTask.FromResult(false);
}
