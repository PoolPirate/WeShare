using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.Common.Models;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api/User/Subscription")]
[ApiController]
public class UserSubscriptionController : ExtendedControllerBase
{
    [HttpGet("Snippets/Id/{userId}")]
    public async Task<ActionResult<PaginatedList<SubscriptionSnippetDto>>> GetShareSnippetsAsync([FromRoute] long userId,
        [FromQuery] ushort page, [FromQuery] ushort pageSize,
        [FromQuery] SubscriptionType? type,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUserSubscriptionSnippetsPaginated
            .Query(new UserId(userId), type, page, pageSize), cancellationToken);

        return result.Status switch
        {
            GetUserSubscriptionSnippetsPaginated.Status.Success => Ok(result.SubscriptionInfos),
            GetUserSubscriptionSnippetsPaginated.Status.UserNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
