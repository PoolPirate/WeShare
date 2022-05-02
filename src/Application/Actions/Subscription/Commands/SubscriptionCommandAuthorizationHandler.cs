using Common.Services;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
[ServiceLevel(1)]
public class SubscriptionCommandAuthorizationHandler : AuthorizationHandler<Subscription, SubscriptionCommandOperation>
{
    [Inject]
    private readonly IShareContext DbContext = null!;

    public override async ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, Subscription entity, SubscriptionCommandOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            SubscriptionCommandOperation.Remove or
            SubscriptionCommandOperation.MarkPostAsReceived
                => authenticatedUser == entity.UserId,

            SubscriptionCommandOperation.Create
                => authenticatedUser == entity.UserId &&
                   await DbContext.Shares
                    .Where(x => x.Id == entity.ShareId)
                    .AllAsync(x => !x.IsPrivate || x.OwnerId == authenticatedUser, cancellationToken),
            _ => throw new InvalidOperationException(),
        };

    public override ValueTask<bool> HandleUnauthenticatedRequestAsync(Subscription entity, SubscriptionCommandOperation operation,
        CancellationToken cancellationToken = default)
        => ValueTask.FromResult(false);
}
