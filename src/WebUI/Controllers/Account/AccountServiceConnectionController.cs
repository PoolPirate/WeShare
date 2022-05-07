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
        [FromQuery] ushort page, [FromQuery] ushort pageSize, [FromQuery] ServiceConnectionType? connectionType,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetServiceConnectionSnippetsPaginated
            .Query(new UserId(userId), page, pageSize, connectionType), cancellationToken);

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
        => serviceConnectionForm.Type switch
        {
            ServiceConnectionType.None => NotFound(),
            ServiceConnectionType.Discord => await CreateDiscordConectionAsync(new UserId(userId), serviceConnectionForm, cancellationToken),
            _ => throw new InvalidOperationException(),
        };

    private async Task<ActionResult> CreateDiscordConectionAsync(UserId userId, CreateServiceConnectionForm serviceConnectionForm,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DiscordConnectionCreateAction
            .Command(userId, serviceConnectionForm.Code), cancellationToken);

        return result.Status switch
        {
            DiscordConnectionCreateAction.Status.Success => Ok(),
            DiscordConnectionCreateAction.Status.UserNotFound => NotFound(nameof(userId)),
            DiscordConnectionCreateAction.Status.InvalidCode => UnprocessableEntity(),
            DiscordConnectionCreateAction.Status.MissingScope => UnprocessableEntity(),
            DiscordConnectionCreateAction.Status.DiscordUserIdCouldNotBeLoaded => UnprocessableEntity(),
            DiscordConnectionCreateAction.Status.FailedAddingToGuild => UnprocessableEntity(),
            DiscordConnectionCreateAction.Status.TargetUserAlreadyLinked => Conflict(),
            _ => throw new InvalidOperationException(),
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
