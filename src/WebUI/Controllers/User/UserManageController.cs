using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;
using WeShare.WebAPI.Forms;
using WeShare.WebAPI.Options;

namespace WeShare.WebAPI.Controllers;

[Route("Api/User")]
[ApiController]
public class UserManageController : ExtendedControllerBase
{
    private readonly LimitingOptions LimitingOptions;

    public UserManageController(LimitingOptions limitingOptions)
    {
        LimitingOptions = limitingOptions;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync([FromBody] UserCreateForm registerForm,
        CancellationToken cancellationToken)
    {
        if (LimitingOptions.DisableRegister)
        {
            return Forbid("Currently disabled");
        }

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
