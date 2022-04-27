using Common.Services;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
[ServiceLevel(1)]
public class UserCommandAuthorizationHandler : AuthorizationHandler<UserId, UserCommandOperation>
{
    [Inject]
    private readonly ICurrentUserService CurrentUserService = null!;

    public override ValueTask<bool> HandleAuthorizationRequestAsync(UserId entity, UserCommandOperation operation, CancellationToken cancellationToken)
    {
        switch (operation)
        {
            case UserCommandOperation.UpdateProfile:
            case UserCommandOperation.RequestPasswordReset:
                return ValueTask.FromResult(true);
        }

        var optionalUserId = CurrentUserService.GetUserId();
        if (!optionalUserId.HasValue)
        {
            return ValueTask.FromResult(false);
        }
        var userId = optionalUserId.Value;

        return operation switch
        {
            UserCommandOperation.UpdateAccount => ValueTask.FromResult(userId == entity),
            _ => throw new NotImplementedException(nameof(operation)),
        };
    }
}
