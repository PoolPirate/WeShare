using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.Common.Models;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api/User/Shares")]
[ApiController]
public class UserSharesController : ExtendedControllerBase
{
    [HttpGet("Liked/Id/{userId}/{page}/{pageSize}")]
    public async Task<ActionResult<PaginatedList<ShareSnippetDto>>> GetLikedShareSnippetsAsync(
        [FromRoute] long userId, [FromRoute] ushort page, [FromRoute] ushort pageSize,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetLikedShareSnippetsPaginated
            .Query(new UserId(userId), page, pageSize), cancellationToken);

        return result.Status switch
        {
            GetLikedShareSnippetsPaginated.Status.Success => Ok(result.ShareSnippets),
            GetLikedShareSnippetsPaginated.Status.UserNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpGet("Liked/Name/{username}/{page}/{pageSize}")]
    public async Task<ActionResult<PaginatedList<ShareSnippetDto>>> GetLikedShareSnippetsAsync(
        [FromRoute] string username, [FromRoute] ushort page, [FromRoute] ushort pageSize,
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
        return await GetLikedShareSnippetsAsync(userId, page, pageSize, cancellationToken);
    }

    [HttpGet("Name/{username}/{page}/{pageSize}")]
    public async Task<ActionResult<PaginatedList<ShareSnippetDto>>> GetShareSnippetsAsync([FromRoute] string username,
        [FromRoute] ushort page, [FromRoute] ushort pageSize,
        [FromQuery] ShareOrdering ordering,
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
        return await GetShareSnippetsAsync(userId, page, pageSize, ordering, cancellationToken);
    }

    [HttpGet("Id/{userId}/{page}/{pageSize}")]
    public async Task<ActionResult<PaginatedList<ShareSnippetDto>>> GetShareSnippetsAsync([FromRoute] long userId,
        [FromRoute] ushort page, [FromRoute] ushort pageSize,
        [FromQuery] ShareOrdering ordering,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetShareSnippetsOrderedPaginated
            .Query(new UserId(userId), ordering, page, pageSize), cancellationToken);

        return result.Status switch
        {
            GetShareSnippetsOrderedPaginated.Status.Success => Ok(result.PopularShareSnippets),
            GetShareSnippetsOrderedPaginated.Status.UserNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
