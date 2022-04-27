using Common.Services;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Command;
[ServiceLevel(1)]
public class ShareCommandAuthorizationHandler : AuthorizationHandler<Share, ShareCommandOperation>
{
    [Inject]
    private readonly ICurrentUserService CurrentUserService = null!;

    public override ValueTask<bool> HandleAuthorizationRequestAsync(Share entity, ShareCommandOperation operation, CancellationToken cancellationToken)
    {
        var optionalUserId = CurrentUserService.GetUserId();
        if (!optionalUserId.HasValue)
        {
            return ValueTask.FromResult(false);
        }

        var userId = optionalUserId.Value;

        return operation switch
        {
            ShareCommandOperation.Create => ValueTask.FromResult(userId == entity.OwnerId),
            ShareCommandOperation.Update => ValueTask.FromResult(userId == entity.OwnerId),
            ShareCommandOperation.Delete => ValueTask.FromResult(userId == entity.OwnerId),
            _ => throw new NotImplementedException(),
        };
    }
}

