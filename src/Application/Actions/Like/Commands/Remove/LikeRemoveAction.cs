using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class LikeRemoveAction
{
    public class Command : IRequest<Result>
    {
        public ShareId ShareId { get; }
        public UserId UserId { get; }

        public Command(ShareId shareId, UserId userId)
        {
            ShareId = shareId;
            UserId = userId;
        }
    }

    public enum Status : byte
    {
        Success,
        LikeNotFound,
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
            var like = await DbContext.Likes
                .Where(x => x.ShareId == request.ShareId && x.OwnerId == request.UserId)
                .SingleOrDefaultAsync(cancellationToken);

            if (like is null)
            {
                return new Result(Status.LikeNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(like, LikeCommandOperation.Remove, cancellationToken);

            using var transaction = await DbContext.BeginTransactionAsync(cancellationToken);

            DbContext.Likes.Remove(like);
            await DbContext.Shares
                .Where(x => x.Id == like.ShareId)
                .UpdateFromQueryAsync(x => new Share() { LikeCount = x.LikeCount - 1 }, cancellationToken);

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.ConcurrencyEntryDeleted, transaction, cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.ConcurrencyEntryDeleted => new Result(Status.LikeNotFound),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

