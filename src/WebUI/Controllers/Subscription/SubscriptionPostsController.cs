﻿using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.Common.Models;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Subscription/Posts")]
[ApiController]
public class SubscriptionPostsController : ExtendedControllerBase
{
    [HttpGet("Unsent/Id/{subscriptionId}")]
    public async Task<ActionResult<PaginatedList<PostSendInfoDto>>> GetUnsentPostSnippetsAsync([FromRoute] long subscriptionId,
        [FromQuery] ushort page, [FromQuery] ushort pageSize,
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

    [HttpGet("Pending/Id/{subscriptionId}")]
    public async Task<ActionResult<object>> GetPendingPostSnippetsAsync([FromRoute] long subscriptionId,
    [FromQuery] ushort page, [FromQuery] ushort pageSize,
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

    [HttpGet("Received/Id/{subscriptionId}")]
    public async Task<ActionResult<PaginatedList<PostSendInfoDto>>> GetReceivedPostSnippetsAsync([FromRoute] long subscriptionId,
    [FromQuery] ushort page, [FromQuery] ushort pageSize,
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
