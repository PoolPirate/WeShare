using Common.Services;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
[ServiceLevel(1)]
public class SentPostCommandAuthorizationHandler : AuthorizationHandler<SentPost, SentPostCommandOperation>
{
    [Inject]
    private readonly IShareContext DbContext = null!;

    public override async ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, SentPost entity, SentPostCommandOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            SentPostCommandOperation.MarkAsReceived
                => await DbContext.Subscriptions
                    .Where(x => x.Id == entity.SubscriptionId)
                    .AllAsync(x => x.UserId == authenticatedUser, cancellationToken: cancellationToken),
            _ => throw new InvalidOperationException(),
        };

    public override ValueTask<bool> HandleUnauthenticatedRequestAsync(SentPost entity, SentPostCommandOperation operation,
        CancellationToken cancellationToken = default)
        => ValueTask.FromResult(false);
}
