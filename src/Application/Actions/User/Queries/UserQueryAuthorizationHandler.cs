using Common.Services;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
[ServiceLevel(1)]
public class UserQueryAuthorizationHandler : AuthorizationHandler<UserId, UserQueryOperation>
{
    [Inject]
    private readonly IShareContext DbContext = null!;

    public override async ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, UserId entity, UserQueryOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            UserQueryOperation.ReadSnippet or
            UserQueryOperation.ReadPublicShareSnippets or
            UserQueryOperation.ReadProfile
                => true,

            UserQueryOperation.ReadLikedShares
                => authenticatedUser == entity ||
                   await DbContext.Users
                    .Where(x => x.Id == entity)
                    .AllAsync(x => x.LikesPublished, cancellationToken),

            UserQueryOperation.ReadSubscriptions or
            UserQueryOperation.ReadAccount
                => authenticatedUser == entity,

            _ => throw new InvalidOperationException(),
        };

    public override async ValueTask<bool> HandleUnauthenticatedRequestAsync(UserId entity, UserQueryOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            UserQueryOperation.ReadSnippet or
            UserQueryOperation.ReadPublicShareSnippets or
            UserQueryOperation.ReadProfile
               => true,

            UserQueryOperation.ReadLikedShares
                => await DbContext.Users
                    .Where(x => x.Id == entity)
                    .AllAsync(x => x.LikesPublished, cancellationToken),

            UserQueryOperation.ReadSubscriptions or
            UserQueryOperation.ReadAccount
                => false,

            _ => throw new InvalidOperationException(),
        };
}
