using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class SentPostMarkAsReceivedAction
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
        NotAllowedForSubscriptionType,
        PostAlreadyReceived,
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
            var sentPost = await DbContext.SentPosts
                .Where(x => x.SubscriptionId == request.SubscriptionId && x.PostId == request.PostId)
                .SingleOrDefaultAsync(cancellationToken);

            if (sentPost is not null)
            {
                return await HandleExistingSentPostAsync(sentPost, cancellationToken);
            }

            var subscriptionData = await DbContext.Subscriptions
            .Where(x => x.Id == request.SubscriptionId)
            .Select(x => new
            {
                x.ShareId,
                x.Type,
                PostShareId = x.Share!.Posts!
                    .Where(x => x.Id == request.PostId)
                    .Select<Post, ShareId?>(x => x.ShareId)
                    .SingleOrDefault()
            })
            .SingleOrDefaultAsync(cancellationToken);

            if (subscriptionData is null)
            {
                return new Result(Status.SubscriptionNotFound);
            }
            if (subscriptionData.PostShareId is null)
            {
                return new Result(Status.PostNotFound);
            }

            sentPost = SentPost.Create(request.PostId, request.SubscriptionId);
            sentPost.SetReceived();
            DbContext.SentPosts.Add(sentPost);

            await Authorizer.EnsureAuthorizationAsync(sentPost, SentPostCommandOperation.MarkAsReceived, cancellationToken);

            if (subscriptionData.ShareId != subscriptionData.PostShareId) //Prevent leaking subscribed share
            {
                return new Result(Status.PostForWrongShare);
            }
            if (!subscriptionData.Type.SupportsMarkAsReceivedAction()) //Prevent leaking subscription type
            {
                return new Result(Status.NotAllowedForSubscriptionType);
            }

            var saveResult = await DbContext.SaveChangesAsync(cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }

        private async Task<Result> HandleExistingSentPostAsync(SentPost sentPost, CancellationToken cancellationToken)
        {
            await Authorizer.EnsureAuthorizationAsync(sentPost, SentPostCommandOperation.MarkAsReceived, cancellationToken);

            if (sentPost.Received)
            {
                return new Result(Status.PostAlreadyReceived);
            }

            var type = await DbContext.Subscriptions
                .Where(x => x.Id == sentPost.SubscriptionId)
                .Select(x => x.Type)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);

            if (!type.SupportsMarkAsReceivedAction())
            {
                return new Result(Status.NotAllowedForSubscriptionType);
            }

            sentPost.SetReceived();

            var saveResult = await DbContext.SaveChangesAsync(cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

