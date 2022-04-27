using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Subscription")]
[ApiController]
public class SubscriptionManageController : ExtendedControllerBase
{
    [HttpPost("Create/{shareId}/{userId}")]
    public async Task<ActionResult<long>> CreateSubscriptionAsync([FromRoute] long shareId, [FromRoute] long userId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SubscriptionCreateAction
            .Command(new ShareId(shareId), new UserId(userId), SubscriptionType.Dashboard), cancellationToken);

        return result.Status switch
        {
            SubscriptionCreateAction.Status.Success => Ok(result.Subscription!.Id),
            SubscriptionCreateAction.Status.ShareNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    } 

    [HttpPost("{subscriptionId}/MarkAsSent/{postId}")]
    public async Task<ActionResult> MarkPostAsSentAsync([FromRoute] long subscriptionId, [FromRoute] long postId, 
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SubscriptionMarkPostAsSentAction
            .Command(new SubscriptionId(subscriptionId), new PostId(postId)), cancellationToken);

        return result.Status switch
        {
            SubscriptionMarkPostAsSentAction.Status.Success => Ok(),
            SubscriptionMarkPostAsSentAction.Status.SubscriptionNotFound => NotFound(),
            SubscriptionMarkPostAsSentAction.Status.SubscriptionAlreadySetHigher => Conflict(),
            SubscriptionMarkPostAsSentAction.Status.PostNotFound => UnprocessableEntity(),
            SubscriptionMarkPostAsSentAction.Status.PostForWrongShare => UnprocessableEntity(),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpDelete("Remove/{subscriptionId}")]
    public async Task<ActionResult> RemoveSubscriptionAsync([FromRoute] long subscriptionId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SubscriptionRemoveAction
            .Command(new SubscriptionId(subscriptionId)), cancellationToken);

        return result.Status switch
        {
            SubscriptionRemoveAction.Status.Success => Ok(),
            SubscriptionRemoveAction.Status.SubscriptionNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
