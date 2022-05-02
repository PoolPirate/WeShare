using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.Common.Models;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api")]
[ApiController]
public class SharePostsInfoController : ExtendedControllerBase
{
    [HttpGet("Shares/{shareId}/Post-Snippets")]
    public async Task<ActionResult<PaginatedList<PostSnippetDto>>> GetPostMetadatasAsync([FromRoute] long shareId,
        [FromQuery] ushort page, [FromQuery] ushort pageSize,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPostSnippetsPaginated
            .Query(new ShareId(shareId), page, pageSize), cancellationToken);

        return result.Status switch
        {
            GetPostSnippetsPaginated.Status.Success => Ok(result.Metadatas),
            GetPostSnippetsPaginated.Status.ShareNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
