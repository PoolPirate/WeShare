using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api")]
[ApiController]
public class SubscriptionInfoController : ExtendedControllerBase
{
    [HttpGet("Subscriptions/{subscriptionId}/Info")]
    public async Task<ActionResult<SubscriptionInfoDto>> GetSubscriptionInfoAsync([FromRoute] long subscriptionId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetSubscriptionInfo
            .Query(new SubscriptionId(subscriptionId)), cancellationToken);

        return result.Status switch
        {
            GetSubscriptionInfo.Status.Success => Ok(result.SubscriptionInfo),
            GetSubscriptionInfo.Status.SubscriptionNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
