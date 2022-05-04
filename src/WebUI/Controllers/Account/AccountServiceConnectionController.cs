using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Commands;
using WeShare.Application.Actions.Queries;
using WeShare.Application.Common.Models;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;
using WeShare.WebAPI.Forms;

namespace WeShare.WebAPI.Controllers;
[Route("Api")]
[ApiController]
public class AccountServiceConnectionController : ExtendedControllerBase
{
    [HttpGet("Accounts/{userId}/ServiceConnection-Snippets")]
    public async Task<ActionResult<PaginatedList<ServiceConnectionSnippetDto>>> GetServiceConnectionSnippetsAsync([FromRoute] long userId,
        [FromQuery] ushort page, [FromQuery] ushort pageSize,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetServiceConnectionSnippetsPaginated
            .Query(new UserId(userId), page, pageSize), cancellationToken);

        return result.Status switch
        {
            GetServiceConnectionSnippetsPaginated.Status.Success => Ok(result.ServiceConnectionSnippets),
            GetServiceConnectionSnippetsPaginated.Status.UserNotFound => NotFound(nameof(userId)),
            _ => throw new InvalidOperationException(),
        };
    }

    [HttpPost("Accounts/{userId}/ServiceConnections")]
    public async Task<ActionResult> CreateServiceConnectionAsync([FromRoute] long userId, [FromBody] CreateServiceConnectionForm serviceConnectionForm,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ServiceConnectionCreateAction
            .Command(new UserId(userId), serviceConnectionForm.Type, serviceConnectionForm.Code), cancellationToken);

        return result.Status switch
        {
            ServiceConnectionCreateAction.Status.Success => Ok(),
            ServiceConnectionCreateAction.Status.UserNotFound => NotFound(nameof(userId)),
            ServiceConnectionCreateAction.Status.InvalidCode => UnprocessableEntity(),
            ServiceConnectionCreateAction.Status.FailedRetrievingUserId => UnprocessableEntity(),
            ServiceConnectionCreateAction.Status.TargetAlreadyLinked => Conflict(),
            _ => throw new NotImplementedException(),
        };
    }

    [HttpDelete("Accounts/{userId}/ServiceConnections/{serviceConnectionId}")]
    public async Task<ActionResult> RemoveServiceConnectionAsync([FromRoute] long userId, [FromRoute] long serviceConnectionId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ServiceConnectionRemoveAction
            .Command(new UserId(userId), new ServiceConnectionId(serviceConnectionId)), cancellationToken);

        return result.Status switch
        {
            ServiceConnectionRemoveAction.Status.Success => Ok(),
            ServiceConnectionRemoveAction.Status.UserNotFound => NotFound(nameof(userId)),
            ServiceConnectionRemoveAction.Status.ServiceConnectionNotFound => NotFound(nameof(serviceConnectionId)),
            _ => throw new InvalidOperationException(),
        };
    }
}
