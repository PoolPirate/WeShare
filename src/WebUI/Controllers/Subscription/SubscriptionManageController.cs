using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;
using WeShare.WebAPI.Forms;

namespace WeShare.WebAPI.Controllers;
[Route("Api")]
[ApiController]
public class SubscriptionManageController : ExtendedControllerBase
{
    [HttpPost("Subscriptions")]
    public async Task<ActionResult<long>> CreateSubscriptionAsync([FromBody] SubscriptionCreateForm createForm,
        CancellationToken cancellationToken) 
        => createForm.Type switch
        {
            SubscriptionType.Dashboard => await CreateDashboardSubscriptionAsync(createForm, cancellationToken),
            SubscriptionType.AndroidPushNotification => throw new NotImplementedException(),
            SubscriptionType.MessagerDiscord => await CreateDiscordSubscriptionAsync(createForm, cancellationToken),
            SubscriptionType.Email => throw new NotImplementedException(),
            SubscriptionType.Webhook => await CreateWebhookSubscriptionAsync(createForm, cancellationToken),
            _ => BadRequest(nameof(createForm.Type)),
        };

    private async Task<ActionResult<long>> CreateDashboardSubscriptionAsync(SubscriptionCreateForm createForm, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DashboardSubscriptionCreateAction
            .Command(SubscriptionName.From(createForm.Name), new ShareId(createForm.ShareId), new UserId(createForm.UserId)), cancellationToken);

        return result.Status switch
        {
            DashboardSubscriptionCreateAction.Status.Success => Ok(result.Subscription!.Id),
            DashboardSubscriptionCreateAction.Status.ShareNotFound => NotFound(nameof(createForm.ShareId)),
            _ => throw new NotImplementedException(),
        };
    }

    private async Task<ActionResult<long>> CreateWebhookSubscriptionAsync(SubscriptionCreateForm createForm, CancellationToken cancellationToken)
    {
        if (createForm.TargetUrl is null || !createForm.TargetUrl.IsAbsoluteUri)
        {
            return BadRequest(nameof(createForm.TargetUrl));
        }

        var result = await Mediator.Send(new WebhookSubscriptionCreateAction
            .Command(SubscriptionName.From(createForm.Name), new ShareId(createForm.ShareId), new UserId(createForm.UserId), createForm.TargetUrl), 
                     cancellationToken);

        return result.Status switch
        {
            WebhookSubscriptionCreateAction.Status.Success => Ok(result.Subscription!.Id),
            WebhookSubscriptionCreateAction.Status.ShareNotFound => NotFound(nameof(createForm.ShareId)),
            _ => throw new NotImplementedException(),
        };
    }

    private async Task<ActionResult<long>> CreateDiscordSubscriptionAsync(SubscriptionCreateForm createForm, CancellationToken cancellationToken)
    {
        if (!createForm.ServiceConnectionId.HasValue)
        {
            return BadRequest(nameof(createForm.ServiceConnectionId));
        }

        var result = await Mediator.Send(new DiscordSubscriptionCreateAction
            .Command(SubscriptionName.From(createForm.Name), new ShareId(createForm.ShareId), new UserId(createForm.UserId), 
                     new ServiceConnectionId(createForm.ServiceConnectionId.Value)), cancellationToken);

        return result.Status switch
        {
            DiscordSubscriptionCreateAction.Status.Success => Ok(result.Subscription!.Id),
            DiscordSubscriptionCreateAction.Status.ShareNotFound => NotFound(nameof(createForm.ShareId)),
            DiscordSubscriptionCreateAction.Status.ServiceConnectionNotFound => NotFound(nameof(createForm.ServiceConnectionId)),
            DiscordSubscriptionCreateAction.Status.DiscordUnavailable => StatusCode(500),
            _ => throw new NotImplementedException(),
        };
    }

    [HttpPost("Subscriptions/{subscriptionId}/Posts/{postId}/Receive")]
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

    [HttpDelete("Subscriptions/{subscriptionId}")]
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
