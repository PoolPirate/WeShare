using Common.Services;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
[ServiceLevel(1)]
public class LikeCommandAuthorizationHandler : AuthorizationHandler<Like, LikeCommandOperation>
{
    [Inject]
    private readonly ICurrentUserService CurrentUserService = null!;

    public override ValueTask<bool> HandleAuthorizationRequestAsync(Like entity, LikeCommandOperation operation, CancellationToken cancellationToken)
    {
        var optionalUserId = CurrentUserService.GetUserId();
        if (!optionalUserId.HasValue)
        {
            return ValueTask.FromResult(false);
        }
        var userId = optionalUserId.Value;

        return operation switch
        {
            LikeCommandOperation.Add or
            LikeCommandOperation.Remove => ValueTask.FromResult(entity.OwnerId == userId),
            _ => throw new NotImplementedException(nameof(operation)),
        };
    }
}
