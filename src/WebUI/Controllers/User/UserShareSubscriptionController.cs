using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.Common.Models;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api")]
[ApiController]
public class UserShareSubscriptionController : ExtendedControllerBase
{
    [HttpGet("Users/{userId}/Shares/{shareId}/Subscription-Snippets")]
    public async Task<ActionResult<PaginatedList<SubscriptionSnippetDto>>> GetSubscriptionSnippetsAsync([FromRoute] long userId, [FromRoute] long shareId,
        [FromQuery] ushort page, [FromQuery] ushort pageSize,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUserShareSubscriptionSnippetsPaginated
            .Query(new UserId(userId), new ShareId(shareId), page, pageSize), cancellationToken);

        return result.Status switch
        {
            GetUserShareSubscriptionSnippetsPaginated.Status.Success => Ok(result.SubscriptionInfos),
            GetUserShareSubscriptionSnippetsPaginated.Status.UserNotFound => NotFound(nameof(userId)),
            GetUserShareSubscriptionSnippetsPaginated.Status.ShareNotFound => NotFound(nameof(shareId)),
            _ => throw new NotImplementedException(),
        };
    }
}
