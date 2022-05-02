using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api")]
[ApiController]
public class CallbackInfoController : ExtendedControllerBase
{
    [HttpGet("Callbacks/{secret}/Info")]
    public async Task<ActionResult<CallbackInfoDto>> GetCallbackInfoAsync([FromRoute] string secret,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCallbackInfo
            .Query(CallbackSecret.From(secret)), cancellationToken);

        return result.Status switch
        {
            GetCallbackInfo.Status.Success => Ok(result.CallbackInfo),
            GetCallbackInfo.Status.CallbackNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
