using MediatR;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class LikeAddAction
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
        AlreadyAdded,
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
            var like = Like.Create(request.UserId, request.ShareId);

            await Authorizer.EnsureAuthorizationAsync(like, LikeCommandOperation.Add, cancellationToken);

            using var transaction = await DbContext.BeginTransactionAsync(cancellationToken);

            DbContext.Likes.Add(like);
            int updateCount = await DbContext.Shares
                .Where(x => x.Id == like.ShareId)
                .Where(x => !x.IsPrivate || x.OwnerId == request.UserId)
                .UpdateFromQueryAsync(x => new Share() { LikeCount = x.LikeCount + 1 }, cancellationToken: cancellationToken);

            if (updateCount == 0) //Share is private and liker is not owner
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new ForbiddenAccessException();
            }

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.DuplicateIndex, transaction: transaction, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.DuplicatePrimaryKey => new Result(Status.AlreadyAdded),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

