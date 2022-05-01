using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.DTOs;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;
public class GetShareUserData
{
    public class Query : IRequest<Result>
    {
        public ShareId ShareId { get; }
        public UserId UserId { get; }

        public Query(ShareId shareId, UserId userId)
        {
            ShareId = shareId;
            UserId = userId;
        }
    }

    public enum Status : byte
    {
        Success,
        ShareNotFound,
        UserNotFound,
    }

    public record Result(Status Status, ShareUserDataDto? ShareUserData = null);

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IAuthorizer Authorizer;

        public Handler(IShareContext dbContext, IAuthorizer authorizer)
        {
            DbContext = dbContext;
            Authorizer = authorizer;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            await Authorizer.EnsureAuthorizationAsync(request.UserId, UserQueryOperation.ReadShareUserdata, cancellationToken);

            bool liked = await DbContext.Likes
                .Where(x => x.ShareId == request.ShareId && x.OwnerId == request.UserId)
                .AnyAsync(cancellationToken);

            bool subscribed = await DbContext.Subscriptions
                .Where(x => x.ShareId == request.ShareId && x.UserId == request.UserId)
                .AnyAsync(cancellationToken);

            var shareUserData = new ShareUserDataDto(liked, subscribed);

            if (liked || subscribed)
            {
                return new Result(Status.Success, shareUserData);
            }

            if (!await DbContext.Shares.AnyAsync(x => x.Id == request.ShareId, cancellationToken))
            {
                return new Result(Status.ShareNotFound);
            }
            if (!await DbContext.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken))
            {
                return new Result(Status.UserNotFound);
            }
            //
            return new Result(Status.Success, shareUserData);
        }
    }
}

