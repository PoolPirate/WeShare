using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;
using WeShare.WebAPI.Forms;

namespace WeShare.WebAPI.Controllers;
[Route("Api")]
[ApiController]
public class CallbackHandlerController : ExtendedControllerBase
{
    [HttpPost("Callback-Management/VerifyEmail")]
    public async Task<ActionResult> HandleEmailVerifyCallbackAsync([FromBody] VerifyEmailForm verifyForm,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CallbackVerifyEmailAction
            .Command(CallbackSecret.From(verifyForm.CallbackSecret)), cancellationToken);

        return result.Status switch
        {
            CallbackVerifyEmailAction.Status.Success => Ok(),
            CallbackVerifyEmailAction.Status.CallbackNotFound => NotFound(),
            CallbackVerifyEmailAction.Status.InvalidCallback => NotFound(),
            CallbackVerifyEmailAction.Status.UserNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpPost("Callback-Management/PasswordReset")]
    public async Task<ActionResult> HandleEmailVerifyCallbackAsync([FromBody] PasswordResetForm resetForm,
    CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CallbackPasswordResetAction
            .Command(CallbackSecret.From(resetForm.CallbackSecret), PlainTextPassword.From(resetForm.Password)),
                     cancellationToken);

        return result.Status switch
        {
            CallbackPasswordResetAction.Status.Success => Ok(),
            CallbackPasswordResetAction.Status.CallbackNotFound => NotFound(),
            CallbackPasswordResetAction.Status.InvalidCallback => NotFound(),
            CallbackPasswordResetAction.Status.UserNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
