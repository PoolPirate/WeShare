using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Like")]
[ApiController]
public class LikeManageController : ExtendedControllerBase
{
    [HttpPost("Add/{shareId}/{userId}")]
    public async Task<ActionResult> AddLikeAsync([FromRoute] long shareId, [FromRoute] long userId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new LikeAddAction
            .Command(new ShareId(shareId), new UserId(userId)), cancellationToken);

        return result.Status switch
        {
            LikeAddAction.Status.Success => Ok(),
            LikeAddAction.Status.AlreadyAdded => Conflict(),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpPost("Remove/{shareId}/{userId}")]
    public async Task<ActionResult> RemoveLikeAsync([FromRoute] long shareId, [FromRoute] long userId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new LikeRemoveAction
            .Command(new ShareId(shareId), new UserId(userId)), cancellationToken);

        return result.Status switch
        {
            LikeRemoveAction.Status.Success => Ok(),
            LikeRemoveAction.Status.LikeNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
