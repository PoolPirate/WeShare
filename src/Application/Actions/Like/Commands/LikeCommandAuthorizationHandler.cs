using Common.Services;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
[ServiceLevel(1)]
public class LikeCommandAuthorizationHandler : AuthorizationHandler<Like, LikeCommandOperation>
{
    [Inject]
    private readonly IShareContext DbContext = null!;

    public override async ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, Like entity, LikeCommandOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            LikeCommandOperation.Add
                => entity.OwnerId == authenticatedUser &&
                   await DbContext.Shares
                    .Where(x => x.Id == entity.ShareId)
                    .AllAsync(x => !x.IsPrivate || x.OwnerId == authenticatedUser, cancellationToken),

            LikeCommandOperation.Remove
                => entity.OwnerId == authenticatedUser,

            _ => throw new InvalidOperationException(),
        };

    public override ValueTask<bool> HandleUnauthenticatedRequestAsync(Like entity, LikeCommandOperation operation,
        CancellationToken cancellationToken = default)
        => ValueTask.FromResult(false);
}
