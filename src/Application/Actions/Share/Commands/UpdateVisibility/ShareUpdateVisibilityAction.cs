using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class ShareUpdateVisibilityAction
{
    public class Command : IRequest<Result>
    {
        public ShareId ShareId { get; }
        public bool IsPrivate { get; }

        public Command(ShareId shareId, bool isPrivate)
        {
            ShareId = shareId;
            IsPrivate = isPrivate;
        }
    }

    public enum Status : byte
    {
        Success,
        ShareNotFound,
        ShareAlreadyHasTargetVisibility,
    }

    public record Result(Status Status);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IAuthorizer Authorizer;

        public Handler(IShareContext dbContext, IAuthorizer authorizer)
        {
            DbContext = dbContext;
            Authorizer = authorizer;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var share = await DbContext.Shares
                .Where(x => x.Id == request.ShareId)
                .SingleOrDefaultAsync(cancellationToken);

            if (share is null)
            {
                return new Result(Status.ShareNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(share, ShareCommandOperation.UpdateVisibility, cancellationToken);

            var transaction = await DbContext.BeginTransactionAsync(cancellationToken);

            share.IsPrivate = request.IsPrivate;

            int removedLikes = await DbContext.Likes
                .Where(x => x.ShareId == request.ShareId)
                .Where(x => x.OwnerId != share.OwnerId)
                .DeleteFromQueryAsync(cancellationToken);

            share.LikeCount -= removedLikes;

            int removedSubscriptions = await DbContext.Subscriptions
                .Where(x => x.ShareId == request.ShareId)
                .Where(x => x.UserId != share.OwnerId)
                .DeleteFromQueryAsync(cancellationToken);

            share.SubscriberCount -= removedSubscriptions;

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.ConcurrencyEntryDeleted, transaction: transaction, cancellationToken: cancellationToken); ;

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.ConcurrencyEntryDeleted => new Result(Status.ShareNotFound),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

