using Common.Services;
using WeShare.Application.Common.Security;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
[ServiceLevel(1)]
public class ShareCommandAuthorizationHandler : AuthorizationHandler<Share, ShareCommandOperation>
{
    public override ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, Share entity, ShareCommandOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            ShareCommandOperation.Create or
            ShareCommandOperation.Update or
            ShareCommandOperation.Delete or
            ShareCommandOperation.UpdateVisibility
                => ValueTask.FromResult(authenticatedUser == entity.OwnerId),
            _ => throw new NotImplementedException(),
        };

    public override ValueTask<bool> HandleUnauthenticatedRequestAsync(Share entity, ShareCommandOperation operation,
        CancellationToken cancellationToken = default)
        => ValueTask.FromResult(false);
}

