using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class SubscriptionMarkPostAsSentAction
{
    public class Command : IRequest<Result>
    {
        public SubscriptionId SubscriptionId { get; }
        public PostId PostId { get; }

        public Command(SubscriptionId subscriptionId, PostId postId)
        {
            SubscriptionId = subscriptionId;
            PostId = postId;
        }
    }

    public enum Status : byte
    {
        Success,
        SubscriptionNotFound,
        SubscriptionAlreadySetHigher,
        PostNotFound,
        PostForWrongShare,
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
            var subscription = await DbContext.Subscriptions
                .Where(x => x.Id == request.SubscriptionId)
                .SingleOrDefaultAsync(cancellationToken);

            if (subscription is null)
            {
                return new Result(Status.SubscriptionNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(subscription, SubscriptionCommandOperation.MarkPostAsSent, cancellationToken);
            
            if ((subscription.LastReceivedPostId?.Value ?? -1) >= request.PostId.Value)
            {
                return new Result(Status.SubscriptionAlreadySetHigher);
            }
            if (!await DbContext.Posts.AnyAsync(x => x.Id == request.PostId, cancellationToken))
            {
                return new Result(Status.PostNotFound);
            }
            if (!await DbContext.Posts.Where(x => x.Id == request.PostId).AllAsync(x => x.ShareId == subscription.ShareId, cancellationToken))
            {
                return new Result(Status.PostForWrongShare);
            }

            subscription.LastReceivedPostId = request.PostId;

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.ConcurrencyEntryDeleted, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.ConcurrencyEntryDeleted => new Result(Status.SubscriptionNotFound),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

