using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api")]
[ApiController]
public class AccountInfoController : ExtendedControllerBase
{
    [HttpGet("Accounts/{userId}/Info")]
    public async Task<ActionResult<AccountInfoDto>> GetAccountInfoAsync([FromRoute] long userId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAccountInfo
            .Query(new UserId(userId)), cancellationToken);

        return result.Status switch
        {
            GetAccountInfo.Status.Success => Ok(result.AccountInfo),
            GetAccountInfo.Status.UserNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpGet("Accounts/ByUsername/{username}/Info")]
    public async Task<ActionResult<AccountInfoDto>> GetAccountInfoAsync([FromRoute] string username,
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
        return await GetAccountInfoAsync(userId, cancellationToken);
    }
}
