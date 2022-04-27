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
        public ShareId ShareId { get; }
        public UserId UserId { get; }
        [EnumDataType(typeof(SubscriptionType))]
        public SubscriptionType Type { get; }

        public Command(ShareId shareId, UserId userId, SubscriptionType type)
        {
            ShareId = shareId;
            UserId = userId;
            Type = type;
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

            var subscription = Subscription.Create(request.Type, request.UserId, request.ShareId, mostRecentPostId);

            await Authorizer.EnsureAuthorizationAsync(subscription, SubscriptionCommandOperation.Create, cancellationToken);

            DbContext.Subscriptions.Add(subscription);

            var saveResult = await DbContext.SaveChangesAsync(cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success, subscription),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

