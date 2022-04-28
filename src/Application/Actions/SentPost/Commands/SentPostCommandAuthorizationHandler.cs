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
    private readonly ICurrentUserService CurrentUserService = null!;
    [Inject]
    private readonly IShareContext DbContext = null!;

    public override async ValueTask<bool> HandleAuthorizationRequestAsync(SentPost entity, SentPostCommandOperation operation, CancellationToken cancellationToken)
    {
        var optionalUserId = CurrentUserService.GetUserId();
        if (!optionalUserId.HasValue)
        {
            return false;
        }
        var userId = optionalUserId.Value;

        return operation switch
        {
            SentPostCommandOperation.MarkAsReceived 
                => await DbContext.Subscriptions
                    .Where(x => x.Id == entity.SubscriptionId)
                    .AllAsync(x => x.UserId == userId, cancellationToken: cancellationToken),
            _ => throw new NotImplementedException(nameof(operation)),
        };
    }
}
