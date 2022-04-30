using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;
using WeShare.WebAPI.Forms;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Subscription")]
[ApiController]
public class SubscriptionManageController : ExtendedControllerBase
{
    [HttpPost("Create")]
    public async Task<ActionResult<long>> CreateSubscriptionAsync([FromBody] SubscriptionCreateForm createForm,
        CancellationToken cancellationToken)
    {
        var request = createForm.Type switch
        {
            SubscriptionType.Dashboard => SubscriptionCreateAction.Command
                .ForDashboard(SubscriptionName.From(createForm.Name), new ShareId(createForm.ShareId), new UserId(createForm.UserId)),
            SubscriptionType.AndroidPushNotification => throw new NotImplementedException(),
            SubscriptionType.MessagerDiscord => throw new NotImplementedException(),
            SubscriptionType.Email => throw new NotImplementedException(),
            SubscriptionType.Webhook => SubscriptionCreateAction.Command
                .ForWebhook(SubscriptionName.From(createForm.Name), new ShareId(createForm.ShareId), new UserId(createForm.UserId), createForm.TargetUrl),
            _ => throw new InvalidOperationException(),
        };

        var result = await Mediator.Send(request, cancellationToken);

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
        var result = await Mediator.Send(new SentPostMarkAsReceivedAction
            .Command(new SubscriptionId(subscriptionId), new PostId(postId)), cancellationToken);

        return result.Status switch
        {
            SentPostMarkAsReceivedAction.Status.Success => Ok(),
            SentPostMarkAsReceivedAction.Status.SubscriptionNotFound => NotFound(nameof(subscriptionId)),
            SentPostMarkAsReceivedAction.Status.NotAllowedForSubscriptionType => UnprocessableEntity(nameof(subscriptionId)),
            SentPostMarkAsReceivedAction.Status.PostAlreadyReceived => Conflict(),
            SentPostMarkAsReceivedAction.Status.PostNotFound => NotFound(nameof(postId)),
            SentPostMarkAsReceivedAction.Status.PostForWrongShare => UnprocessableEntity(nameof(postId)),
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
