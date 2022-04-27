using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Profile")]
[ApiController]
public class ProfileInfoController : ExtendedControllerBase
{
    [HttpGet("Id/{userId}")]
    public async Task<ActionResult<ProfileInfoDto>> GetProfileInfoAsync([FromRoute] long userId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProfileInfo
            .Query(new UserId(userId)), cancellationToken);

        return result.Status switch
        {
            GetProfileInfo.Status.Success => Ok(result.ProfileInfo),
            GetProfileInfo.Status.UserNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpGet("Name/{username}")]
    public async Task<ActionResult<ProfileInfoDto>> GetProfileInfoAsync([FromRoute] string username,
        CancellationToken cancellationToken)
    {
        var idResult = await Mediator.Send(new GetUserIdByUsername.Query(Username.From(username.ToLower())), cancellationToken);

        switch (idResult.Status)
        {
            case GetUserIdByUsername.Status.Success: break;
            case GetUserIdByUsername.Status.UserNotFound: return NotFound();
            default: throw new InvalidOperationException();
        }

        long userId = idResult.UserId!.Value.Value;
        return await GetProfileInfoAsync(userId, cancellationToken);
    }
}
