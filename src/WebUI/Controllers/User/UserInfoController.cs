using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api")]
[ApiController]
public class UserInfoController : ExtendedControllerBase
{
    [HttpGet("Users/ByUsername/{username}/Snippet")]
    public async Task<ActionResult<UserSnippetDto>> GetUserSnippetAsync([FromRoute] string username,
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
        return await GetUserSnippetAsync(userId, cancellationToken);
    }

    [HttpGet("Users/{userId}/Snippet")]
    public async Task<ActionResult<UserSnippetDto>> GetUserSnippetAsync([FromRoute] long userId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUserSnippet
            .Query(new UserId(userId)), cancellationToken);

        return result.Status switch
        {
            GetUserSnippet.Status.Success => Ok(result.UserSnippet),
            GetUserSnippet.Status.UserNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
