using Common.Services;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
[ServiceLevel(1)]
public class PostFilterCommandAuthorizationHandler : AuthorizationHandler<PostFilter, PostFilterCommandOperation>
{
    [Inject]
    private readonly IShareContext DbContext = null!;

    public override async ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, PostFilter entity, PostFilterCommandOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            PostFilterCommandOperation.Create or
            PostFilterCommandOperation.Remove
                => await DbContext.Shares
                    .Where(x => x.Id == entity.ShareId)
                    .AllAsync(x => x.OwnerId == authenticatedUser, cancellationToken),
            _ => throw new InvalidOperationException(),
        };

    public override ValueTask<bool> HandleUnauthenticatedRequestAsync(PostFilter entity, PostFilterCommandOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            PostFilterCommandOperation.Create or
            PostFilterCommandOperation.Remove 
                => ValueTask.FromResult(false),
            _ => throw new InvalidOperationException(),
        };
}
