using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;
using WeShare.WebAPI.Forms;

namespace WeShare.WebAPI.Controllers;

[Route("Api/User")]
[ApiController]
public class UserManageController : ExtendedControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync([FromBody] UserCreateForm registerForm,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UserCreateAction
            .Command(Username.From(registerForm.Username.ToLower()), Nickname.From(registerForm.Username),
                     registerForm.Email, PlainTextPassword.From(registerForm.Password)), cancellationToken);

        return result.Status switch
        {
            UserCreateAction.Status.Success => Ok(),
            UserCreateAction.Status.UsernameTaken => Conflict(),
            UserCreateAction.Status.EmailTaken => Conflict(),
            _ => throw new InvalidOperationException(),
        };
    }
}
