using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Shares")]
[ApiController]
public class ShareInfoController : ExtendedControllerBase
{
    [HttpGet("{shareId}/Snippet")]
    public async Task<ActionResult<ShareSnippetDto>> GetShareSnippetAsync([FromRoute] long shareId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetShareSnippet
            .Query(new ShareId(shareId)), cancellationToken);

        return result.Status switch
        {
            GetShareSnippet.Status.Success => Ok(result.ShareSnippet),
            GetShareSnippet.Status.ShareNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpGet("{shareId}/Data")]
    public async Task<ActionResult<ShareDataDto>> GetShareDataAsync([FromRoute] long shareId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetShareData
            .Query(new ShareId(shareId)), cancellationToken);

        return result.Status switch
        {
            GetShareData.Status.Success => Ok(result.ShareData),
            GetShareData.Status.ShareNotFound => NotFound(nameof(shareId)),
            GetShareData.Status.OwnerNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpGet("{shareId}/UserData/{userId}")]
    public async Task<ActionResult<ShareUserDataDto>> GetShareUserDataAsync([FromRoute] long shareId, [FromRoute] long userId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetShareUserData
            .Query(new ShareId(shareId), new UserId(userId)), cancellationToken);

        return result.Status switch
        {
            GetShareUserData.Status.Success => Ok(result.ShareUserData),
            GetShareUserData.Status.ShareNotFound => NotFound(shareId),
            GetShareUserData.Status.UserNotFound => NotFound(userId),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpGet("{shareId}/Secrets")]
    public async Task<ActionResult<ShareSecretsDto>> GetShareSecretsAsync([FromRoute] long shareId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetShareSecrets
            .Query(new ShareId(shareId)), cancellationToken);

        return result.Status switch
        {
            GetShareSecrets.Status.Success => Ok(result.ShareSecrets),
            GetShareSecrets.Status.ShareNotFound => NotFound(shareId),
            _ => throw new InvalidOperationException(),
        };
    }
}
