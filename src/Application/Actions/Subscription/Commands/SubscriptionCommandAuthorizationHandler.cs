using Common.Services;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
[ServiceLevel(1)]
public class SubscriptionCommandAuthorizationHandler : AuthorizationHandler<Subscription, SubscriptionCommandOperation>
{
    [Inject]
    private readonly ICurrentUserService CurrentUserService = null!;

    public override ValueTask<bool> HandleAuthorizationRequestAsync(Subscription entity, SubscriptionCommandOperation operation, CancellationToken cancellationToken)
    {
        var optionalUserId = CurrentUserService.GetUserId();
        if (!optionalUserId.HasValue)
        {
            return ValueTask.FromResult(false);
        }
        var userId = optionalUserId.Value;

        return operation switch
        {
            SubscriptionCommandOperation.Create or
            SubscriptionCommandOperation.Remove or
            SubscriptionCommandOperation.MarkPostAsReceived
                => ValueTask.FromResult(userId == entity.UserId),
            _ => throw new NotImplementedException(nameof(operation)),
        };
    }
}
