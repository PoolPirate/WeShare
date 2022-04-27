using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.Common.Models;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Share/Posts")]
[ApiController]
public class SharePostsInfoController : ExtendedControllerBase
{
    [HttpGet("Metadata/{shareId}/{page}/{pageSize}")]
    public async Task<ActionResult<PaginatedList<PostMetadataDto>>> GetPostMetadatasAsync([FromRoute] long shareId,
        [FromRoute] ushort page, [FromRoute] ushort pageSize,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPostMetadatasPaginated
            .Query(new ShareId(shareId), page, pageSize), cancellationToken);

        return result.Status switch
        {
            GetPostMetadatasPaginated.Status.Success => Ok(result.Metadatas),
            GetPostMetadatasPaginated.Status.ShareNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
