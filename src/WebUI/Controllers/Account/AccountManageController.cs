using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;
using WeShare.WebAPI.Forms;

namespace WeShare.WebAPI.Controllers;
[Route("Api")]
[ApiController]
public class AccountManageController : ExtendedControllerBase
{
    [HttpPatch("Accounts/{userId}")]
    public async Task<ActionResult> UpdateAccountAsync([FromRoute] long userId, [FromBody] AccountUpdateForm updateForm)
    {
        var result = await Mediator.Send(new AccountUpdateAction
            .Command(new UserId(userId), updateForm.Username, updateForm.Email));

        return result.Status switch
        {
            AccountUpdateAction.Status.Success => Ok(),
            AccountUpdateAction.Status.UserNotFound => NotFound(),
            AccountUpdateAction.Status.UsernameTaken => Conflict(),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpPost("Account-Management/RequestPasswordReset")]
    public async Task<ActionResult> RequestPasswordResetAsync([FromBody] AccountRequestPassswordResetForm resetForm)
    {
        var result = await Mediator.Send(new UserRequestPasswordResetAction
            .Command(resetForm.Email));

        return result.Status switch
        {
            UserRequestPasswordResetAction.Status.Success => Ok(),
            UserRequestPasswordResetAction.Status.EmailNotRegistered => Ok(),
            UserRequestPasswordResetAction.Status.LastRequestStillValid => Ok(),
            _ => throw new InvalidOperationException(),
        };
    }
}
