using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;
using WeShare.WebAPI.Forms;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Subscription")]
[ApiController]
public class SubscriptionManageController : ExtendedControllerBase
{
    [HttpPost("Create/{shareId}/{userId}")]
    public async Task<ActionResult<long>> CreateSubscriptionAsync([FromBody] SubscriptionCreateForm createForm,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SubscriptionCreateAction
            .Command(createForm.Type, SubscriptionName.From(createForm.Name),
            new ShareId(createForm.ShareId), new UserId(createForm.UserId)), cancellationToken);

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
        var result = await Mediator.Send(new SubscriptionMarkPostAsReceivedAction
            .Command(new SubscriptionId(subscriptionId), new PostId(postId)), cancellationToken);

        return result.Status switch
        {
            SubscriptionMarkPostAsReceivedAction.Status.Success => Ok(),
            SubscriptionMarkPostAsReceivedAction.Status.SubscriptionNotFound => NotFound(),
            SubscriptionMarkPostAsReceivedAction.Status.SubscriptionAlreadySetHigher => Conflict(),
            SubscriptionMarkPostAsReceivedAction.Status.PostNotFound => UnprocessableEntity(),
            SubscriptionMarkPostAsReceivedAction.Status.PostForWrongShare => UnprocessableEntity(),
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
