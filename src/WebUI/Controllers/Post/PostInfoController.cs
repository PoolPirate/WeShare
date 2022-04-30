﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeShare.Application.Actions.Queries;
using WeShare.Application.DTOs;
using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Controllers;
[Route("Api/Post")]
[ApiController]
public class PostInfoController : ExtendedControllerBase
{
    [HttpGet("Snippet/Id/{postId}")]
    public async Task<ActionResult<PostSnippetDto>> GetPostSnippetAsync([FromRoute] long postId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPostSnippet
            .Query(new PostId(postId)), cancellationToken);

        return result.Status switch
        {
            GetPostSnippet.Status.Success => Ok(result.PostSnippet),
            GetPostSnippet.Status.PostNotFound => NotFound(),
            _ => throw new InvalidOperationException(),
        };
    }
}