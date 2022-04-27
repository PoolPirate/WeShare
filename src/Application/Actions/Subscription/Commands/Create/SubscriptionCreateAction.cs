using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class SubscriptionCreateAction
{
    public class Command : IRequest<Result>
    {
        [EnumDataType(typeof(SubscriptionType))]
        public SubscriptionType Type { get; }
        public SubscriptionName Name { get; }
        public ShareId ShareId { get; }
        public UserId UserId { get; }


        public Command(SubscriptionType type, SubscriptionName name, ShareId shareId, UserId userId)
        {
            Type = type;
            Name = name;
            ShareId = shareId;
            UserId = userId;
        }
    }

    public enum Status : byte
    {
        Success,
        ShareNotFound,
    }

    public record Result(Status Status, Subscription? Subscription = null);

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
            var mostRecentPostId = await DbContext.Posts
                .Where(x => x.ShareId == request.ShareId)
                .OrderByDescending(x => x.CreatedAt)
                .Select<Post, PostId?>(x => x.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (mostRecentPostId == default &&
                !await DbContext.Shares.AnyAsync(x => x.Id == request.ShareId, cancellationToken))
            {
                return new Result(Status.ShareNotFound);
            }

            var subscription = Subscription.Create(request.Type, request.Name, request.UserId, request.ShareId, mostRecentPostId);

            await Authorizer.EnsureAuthorizationAsync(subscription, SubscriptionCommandOperation.Create, cancellationToken);

            var transaction = await DbContext.BeginTransactionAsync(cancellationToken);

            DbContext.Subscriptions.Add(subscription);
            await DbContext.Shares
                .Where(x => x.Id == request.ShareId)
                .UpdateFromQueryAsync(x => new Share() { LikeCount = x.LikeCount + 1 }, cancellationToken: cancellationToken);

            var saveResult = await DbContext.SaveChangesAsync(transaction: transaction, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success, subscription),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

