﻿using Common.Services;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
[ServiceLevel(1)]
public class UserQueryAuthorizationHandler : AuthorizationHandler<UserId, UserQueryOperation>
{
    [Inject]
    private readonly IShareContext DbContext = null!;
    [Inject]
    private readonly ICurrentUserService CurrentUserService = null!;

    public override async ValueTask<bool> HandleAuthorizationRequestAsync(UserId entity, UserQueryOperation operation, CancellationToken cancellationToken)
    {
        switch (operation)
        {
            case UserQueryOperation.ReadSnippet:
            case UserQueryOperation.ReadProfile:
            case UserQueryOperation.ReadPopularShares:
                return true;
        }

        var optionalUserId = CurrentUserService.GetUserId();
        if (!optionalUserId.HasValue)
        {
            return operation switch
            {
                UserQueryOperation.ReadLikedShares => await DbContext.Users
                                    .Where(x => x.Id == entity)
                                    .AllAsync(x => x.LikesPublished, cancellationToken),
                _ => false,
            };
        }
        var userId = optionalUserId.Value;

        return operation switch
        {
            UserQueryOperation.ReadLikedShares or
            UserQueryOperation.ReadAccount or
            UserQueryOperation.ReadSubscriptions or
            UserQueryOperation.ReadShareUserdata
                => userId == entity,
            _ => throw new NotImplementedException(nameof(operation)),
        };
    }
}
