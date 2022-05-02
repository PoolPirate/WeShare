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

    public override async ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, SubscriptionId entity, SubscriptionQueryOperation operation,
        CancellationToken cancellationToken = default) 
        => operation switch
        {
            SubscriptionQueryOperation.ReadUnreceivedPosts or
            SubscriptionQueryOperation.ReadInfo or
            SubscriptionQueryOperation.ReadPendingPosts or
            SubscriptionQueryOperation.ReadReceivedPosts
                => await DbContext.Subscriptions.Where(x => x.Id == entity).AllAsync(x => x.UserId == authenticatedUser, cancellationToken),
            _ => throw new NotImplementedException(nameof(operation)),
        };

    public override ValueTask<bool> HandleUnauthenticatedRequestAsync(SubscriptionId entity, SubscriptionQueryOperation operation,
        CancellationToken cancellationToken = default)
        => ValueTask.FromResult(false);
}
