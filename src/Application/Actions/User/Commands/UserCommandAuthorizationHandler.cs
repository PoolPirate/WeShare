using Common.Services;
using WeShare.Application.Common.Security;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
[ServiceLevel(1)]
public class UserCommandAuthorizationHandler : AuthorizationHandler<UserId, UserCommandOperation>
{
    public override ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, UserId entity, UserCommandOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            UserCommandOperation.UpdateProfile or
            UserCommandOperation.RequestPasswordReset
                => ValueTask.FromResult(true),

            UserCommandOperation.UpdateAccount
                => ValueTask.FromResult(authenticatedUser == entity),

            _ => throw new InvalidOperationException(),
        };

    public override ValueTask<bool> HandleUnauthenticatedRequestAsync(UserId entity, UserCommandOperation operation,
        CancellationToken cancellationToken = default)
        => ValueTask.FromResult(false);
}
