using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.DTOs;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Callback")]
[ApiController]
public class CallbackInfoController : ExtendedControllerBase
{
    [HttpGet("Info/Secret/{secret}")]
    public async Task<ActionResult<CallbackInfoDto>> GetCallbackInfoAsync([FromRoute] string callbackSecret,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCallbackInfo
            .Query(callbackSecret), cancellationToken);

        return result.Status switch
        {
            GetCallbackInfo.Status.Success => Ok(result.CallbackInfo),
            GetCallbackInfo.Status.CallbackNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
