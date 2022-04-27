using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;
using WeShare.WebAPI.Forms;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Profile")]
[ApiController]
public class ProfileManageController : ExtendedControllerBase
{
    [HttpPost("{userId}/Update")]
    public async Task<ActionResult> UpdateProfileAsync([FromRoute] long userId, [FromBody] ProfileUpdateForm profileUpdateForm,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ProfileUpdateAction
            .Command(new UserId(userId), Nickname.From(profileUpdateForm.Nickname), profileUpdateForm.LikesPublished), cancellationToken);

        return result.Status switch
        {
            ProfileUpdateAction.Status.Success => Ok(),
            ProfileUpdateAction.Status.UserNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
