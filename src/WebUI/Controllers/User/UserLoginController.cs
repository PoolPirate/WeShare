using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;
using WeShare.WebAPI.Forms;

namespace WeShare.WebAPI.Controllers;

[Route("Api")]
[ApiController]
public class UserLoginController : ExtendedControllerBase
{
    [HttpPost("User-Management/Login")]
    public async Task<ActionResult<UserLoginDto>> LoginAsync([FromBody] UserLoginForm loginForm,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UserLoginAction
            .Command(Username.From(loginForm.Username.ToLower()), PlainTextPassword.From(loginForm.Password)), cancellationToken);

        return result.Status switch
        {
            UserLoginAction.Status.Success => Ok(result.UserLogin),
            UserLoginAction.Status.UserNotFound => Unauthorized(),
            UserLoginAction.Status.WrongPassword => Unauthorized(),
            _ => throw new InvalidOperationException(),
        };
    }
}
