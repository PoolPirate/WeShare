using Common.Services;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;

[ServiceLevel(1)]
public class PostQueryAuthorizationHandler : AuthorizationHandler<PostId, PostQueryOperation>
{
    [Inject]
    private readonly IShareContext DbContext = null!;

    public override async ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, PostId entity, PostQueryOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            PostQueryOperation.ReadContent
                => await DbContext.Posts
                    .Where(x => x.Id == entity)
                    .AllAsync(x => !x.Share!.IsPrivate || x.Share!.OwnerId == authenticatedUser, cancellationToken),

            _ => throw new InvalidOperationException(),
        };

    public override ValueTask<bool> HandleUnauthenticatedRequestAsync(PostId entity, PostQueryOperation operation,
        CancellationToken cancellationToken = default)
        => ValueTask.FromResult(false);
}
