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

    [Inject]
    private readonly ICurrentUserService CurrentUserService = null!;

    public override async ValueTask<bool> HandleAuthorizationRequestAsync(ShareId shareId, ShareQueryOperation operation, CancellationToken cancellationToken)
    {
        switch (operation)
        {
            case ShareQueryOperation.ReadData:
            case ShareQueryOperation.ReadPosts:
                return true;
        }

        var optionalUserId = CurrentUserService.GetUserId();
        if (!optionalUserId.HasValue)
        {
            return false;
        }
        var userId = optionalUserId.Value;

        return operation switch
        {
            ShareQueryOperation.ReadSecrets =>
                await DbContext.Shares
                 .Where(x => x.Id == shareId)
                 .AllAsync(x => x.OwnerId == userId, cancellationToken),
            ShareQueryOperation.ReadLikes =>
                await DbContext.Shares
                 .Where(x => x.Id == shareId)
                 .AllAsync(x => x.OwnerId == userId || x.Owner!.LikesPublished, cancellationToken),
            _ => throw new NotImplementedException(nameof(operation)),
        };
    }
}
