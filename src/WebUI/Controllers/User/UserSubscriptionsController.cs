using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.Common.Models;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api/User/Subscriptions")]
[ApiController]
public class UserSubscriptionsController : ExtendedControllerBase
{
    [HttpGet("Id/{userId}/{page}/{pageSize}")]
    public async Task<ActionResult<PaginatedList<SubscriptionInfoDto>>> GetShareSnippetsAsync([FromRoute] long userId,
        [FromRoute] ushort page, [FromRoute] ushort pageSize,
        [FromQuery] ShareOrdering ordering,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetSubscriptionInfosPaginated
            .Query(new UserId(userId), page, pageSize), cancellationToken);

        return result.Status switch
        {
            GetSubscriptionInfosPaginated.Status.Success => Ok(result.SubscriptionInfos),
            GetSubscriptionInfosPaginated.Status.UserNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
