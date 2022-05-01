using Common.Services;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
[ServiceLevel(1)]
public class SubscriptionQueryAuthorizationHandler : AuthorizationHandler<SubscriptionId, SubscriptionQueryOperation>
{
    [Inject]
    private readonly IShareContext DbContext = null!;

    [Inject]
    private readonly ICurrentUserService CurrentUserService = null!;

    public override async ValueTask<bool> HandleAuthorizationRequestAsync(SubscriptionId entity, SubscriptionQueryOperation operation, CancellationToken cancellationToken)
    {
        var optionalUserId = CurrentUserService.GetUserId();
        if (!optionalUserId.HasValue)
        {
            return false;
        }
        var userId = optionalUserId.Value;

        return operation switch
        {
            SubscriptionQueryOperation.ReadUnreceivedPosts or
            SubscriptionQueryOperation.ReadInfo or
            SubscriptionQueryOperation.ReadPendingPosts or
            SubscriptionQueryOperation.ReadReceivedPosts
                => await DbContext.Subscriptions.Where(x => x.Id == entity).AllAsync(x => x.UserId == userId, cancellationToken),
            _ => throw new NotImplementedException(nameof(operation)),
        };
    }
}
