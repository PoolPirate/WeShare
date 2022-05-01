using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.Common.Models;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Subscription/Posts")]
[ApiController]
public class SubscriptionPostsController : ExtendedControllerBase
{
    [HttpGet("Unsent/Id/{subscriptionId}/{page}/{pageSize}")]
    public async Task<ActionResult<PaginatedList<PostSnippetDto>>> GetUnsentPostSnippetsAsync([FromRoute] long subscriptionId,
        [FromRoute] ushort page, [FromRoute] ushort pageSize,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetSubscriptionUnsentPostSnippetsPaginated
            .Query(new SubscriptionId(subscriptionId), page, pageSize), cancellationToken);

        return result.Status switch
        {
            GetSubscriptionUnsentPostSnippetsPaginated.Status.Success => Ok(result.PostSnippets),
            GetSubscriptionUnsentPostSnippetsPaginated.Status.SubscriptionNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpGet("Pending/Id/{subscriptionId}/{page}/{pageSize}")]
    public async Task<ActionResult<PaginatedList<SentPostInfoDto>>> GetPendingPostSnippetsAsync([FromRoute] long subscriptionId,
    [FromRoute] ushort page, [FromRoute] ushort pageSize,
    CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetSubscriptionPendingPostSnippetsPaginated
            .Query(new SubscriptionId(subscriptionId), page, pageSize), cancellationToken);

        return result.Status switch
        {
            GetSubscriptionPendingPostSnippetsPaginated.Status.Success => Ok(result.PostSnippets),
            GetSubscriptionPendingPostSnippetsPaginated.Status.SubscriptionNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpGet("Received/Id/{subscriptionId}/{page}/{pageSize}")]
    public async Task<ActionResult<PaginatedList<SentPostInfoDto>>> GetReceivedPostSnippetsAsync([FromRoute] long subscriptionId,
    [FromRoute] ushort page, [FromRoute] ushort pageSize,
    CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetSubscriptionReceivedPostSnippetsPaginated
            .Query(new SubscriptionId(subscriptionId), page, pageSize), cancellationToken);

        return result.Status switch
        {
            GetSubscriptionReceivedPostSnippetsPaginated.Status.Success => Ok(result.PostSnippets),
            GetSubscriptionReceivedPostSnippetsPaginated.Status.SubscriptionNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
