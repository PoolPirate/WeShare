﻿using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;
using WeShare.WebAPI.Forms;

namespace WeShare.WebAPI.Controllers;
[Route("Api")]
[ApiController]
public class ShareManageController : ExtendedControllerBase
{
    [HttpPost("Users/{userId}/Shares")]
    public async Task<ActionResult<long>> CreateShareAsync([FromRoute] long userId, [FromBody] ShareCreateForm createForm,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ShareCreateAction
            .Command(new UserId(userId), ShareName.From(createForm.Name), createForm.IsPrivate,
                     createForm.Description, createForm.Readme), cancellationToken);

        return result.Status switch
        {
            ShareCreateAction.Status.Success => Ok(result.ShareId!.Value),
            ShareCreateAction.Status.NameTaken => Conflict(),
            _ => throw new InvalidOperationException()
        };
    }

    [HttpPatch("Shares/{shareId}")]
    public async Task<ActionResult> UpdateShareAsync([FromRoute] long shareId, [FromBody] ShareUpdateForm updateForm,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ShareUpdateAction
            .Command(new ShareId(shareId), updateForm.Name is not null ? ShareName.From(updateForm.Name) : null,
                     updateForm.Description, updateForm.Readme), cancellationToken);

        return result.Status switch
        {
            ShareUpdateAction.Status.Success => Ok(),
            ShareUpdateAction.Status.ShareNotFound => NotFound(),
            ShareUpdateAction.Status.ShareNameTaken => Conflict(),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpPatch("Shares/{shareId}/Visibility")]
    public async Task<ActionResult> UpdateShareVisibilityAsync([FromRoute] long shareId, [FromBody] ShareUpdateVisibilityForm updateForm,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ShareUpdateVisibilityAction
            .Command(new ShareId(shareId), updateForm.IsPrivate), cancellationToken);

        return result.Status switch
        {
            ShareUpdateVisibilityAction.Status.Success => Ok(),
            ShareUpdateVisibilityAction.Status.ShareNotFound => NotFound(),
            ShareUpdateVisibilityAction.Status.ShareAlreadyHasTargetVisibility => Conflict(),
            _ => throw new NotImplementedException(),
        };
    }

    [HttpDelete("Shares/{shareId}")]
    public async Task<ActionResult> DeleteShareAsync([FromRoute] long shareId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ShareDeleteAction
            .Command(new ShareId(shareId)), cancellationToken);

        return result.Status switch
        {
            ShareDeleteAction.Status.Success => Ok(),
            ShareDeleteAction.Status.ShareNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}
