using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api")]
[ApiController]
public class PostManageController : ExtendedControllerBase
{
    [HttpPost("Post-Management/{shareSecret}")]
    public async Task<ActionResult> SubmitPostAsync([FromRoute] string shareSecret)
    {
        var result = await Mediator.Send(new PostSubmitAction
            .Command(ShareSecret.From(shareSecret), Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToArray()), Request.Body));

        return result.Status switch
        {
            PostSubmitAction.Status.Success => Ok(),
            PostSubmitAction.Status.ShareNotFound => NotFound(),
            _ => throw new NotImplementedException(),
        };
    }
}
