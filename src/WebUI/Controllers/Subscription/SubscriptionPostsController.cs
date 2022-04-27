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
    public async Task<ActionResult<PaginatedList<PostSnippetDto>>> GetUnsentPostMetadatasAsync([FromRoute] long subscriptionId,
        [FromRoute] ushort page, [FromRoute] ushort pageSize,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetSubscriptionUnsentPostMetadatasPaginated
            .Query(new SubscriptionId(subscriptionId), page, pageSize), cancellationToken);

        return result.Status switch
        {
            GetSubscriptionUnsentPostMetadatasPaginated.Status.Success => Ok(result.PostMetadatas),
            GetSubscriptionUnsentPostMetadatasPaginated.Status.SubscriptionNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
