using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Post")]
[ApiController]
public class PostContentInfoController : ExtendedControllerBase
{
    [HttpGet("Content/{postId}")]
    public async Task<ActionResult<PostContent>> GetPostContentAsync([FromRoute] long postId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPostContent
            .Query(new PostId(postId)), cancellationToken);

        return result.Status switch
        {
            GetPostContent.Status.Success => Ok(result.PostContent),
            GetPostContent.Status.ContentNotFound => NoContent(),
            GetPostContent.Status.PostNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
