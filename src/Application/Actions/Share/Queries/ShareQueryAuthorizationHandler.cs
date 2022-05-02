using Common.Services;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
[ServiceLevel(1)]
public class ShareQueryAuthorizationHandler : AuthorizationHandler<ShareId, ShareQueryOperation>
{
    [Inject]
    private readonly IShareContext DbContext = null!;

    public override async ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, ShareId entity, ShareQueryOperation operation, 
        CancellationToken cancellationToken = default) 
        => operation switch
        {
            ShareQueryOperation.ReadSecrets
                => await DbContext.Shares
                    .Where(x => x.Id == entity)
                    .AllAsync(x => x.OwnerId == authenticatedUser, cancellationToken),

            ShareQueryOperation.ReadSnippet or
            ShareQueryOperation.ReadData or
            ShareQueryOperation.ReadPosts or
            ShareQueryOperation.ReadUserData
                => await DbContext.Shares
                    .Where(x => x.Id == entity)
                    .AllAsync(x => !x.IsPrivate || x.OwnerId == authenticatedUser, cancellationToken),

            _ => throw new InvalidOperationException(),
        };

    public override async ValueTask<bool> HandleUnauthenticatedRequestAsync(ShareId shareId, ShareQueryOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            ShareQueryOperation.ReadSecrets or
            ShareQueryOperation.ReadUserData
                => false,

            ShareQueryOperation.ReadSnippet or
            ShareQueryOperation.ReadData or
            ShareQueryOperation.ReadPosts
                => await DbContext.Shares
                    .Where(x => x.Id == shareId)
                    .AllAsync(x => !x.IsPrivate, cancellationToken),

            _ => throw new InvalidOperationException(),
        };
}
