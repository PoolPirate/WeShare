using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeShare.Application.Actions.Tasks;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;

public sealed class PostPublisher<TSubsbcription> : Scoped
    where TSubsbcription : Subscription
{
    [Inject]
    private readonly IShareContext DbContext = null!;
    [Inject]
    private readonly ISusbcriptionPostPublisher<TSubsbcription> Publisher = null!;
    [Inject]
    private readonly IPostStorage PostStorage = null!;
    [Inject]
    private readonly IDispatcher Dispatcher = null!;
    [Inject]
    private readonly ILogger<PostPublisher<TSubsbcription>> Logger = null!;
    [Inject]
    private readonly IServiceProvider Provider = null!;

    private bool RequiresRetry = false;

    public async Task PublishToSubscribersAsync(PostId postId, CancellationToken cancellationToken)
    {
        var post = await DbContext.Posts
               .AsNoTracking()
               .Where(x => x.Id == postId)
               .SingleOrDefaultAsync(cancellationToken);

        if (post is null)
        {
            Logger.LogError("Error while publishing post to {subscriberType}s. Post not found: PostId={postId}", nameof(TSubsbcription), postId);
            return;
        }

        var content = await PostStorage.LoadAsync(post.Id, cancellationToken);

        if (content is null)
        {
            throw new InvalidOperationException($"Loading post content failed: PostId={post.Id}");
        }

        var options = new ParallelOptions()
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = Publisher.DegreeOfParallelism,
        };

        var subscriptionChunks = await GetTargetSubscriptionChunks(postId, post.CreatedAt, post.ShareId, cancellationToken);

        await Parallel.ForEachAsync(subscriptionChunks, options,
            (subscribers, cancellationToken) => PublishToSubscriberChunkAsync(post, content, subscribers, cancellationToken));

        if (RequiresRetry)
        {
            var retryTask = new PostPublishTask.Command<TSubsbcription>(postId);
            Dispatcher.Schedule(retryTask, $"Retry Publish Post To {Publisher.Type}: {postId}", TimeSpan.FromSeconds(30));
        }

        //Ignore errors caused by people unsubscribing during execution of this
        var saveResult = await DbContext.SaveChangesAsync(discardConcurrentDeletedEntries: true, cancellationToken: cancellationToken);

        if (saveResult.Status != DbStatus.Success)
        {
            throw new UnhandledDbStatusException(saveResult);
        }
    }

    private async ValueTask PublishToSubscriberChunkAsync(Post post, PostContent postContent, TSubsbcription[] subscriptions,
        CancellationToken cancellationToken)
    {
        using var scope = Provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IShareContext>();
        var chunkPublisher = scope.ServiceProvider.GetRequiredService<ISusbcriptionPostPublisher<TSubsbcription>>();

        foreach (var subscription in subscriptions)
        {
            try
            {
                var sentPost = await dbContext.SentPosts
                    .Where(x => x.SubscriptionId == subscription.Id)
                    .Where(x => x.PostId == post.Id)
                    .SingleOrDefaultAsync(cancellationToken);

                if (sentPost is null)
                {
                    sentPost = SentPost.Create(post.Id, subscription.Id);
                    dbContext.SentPosts.Add(sentPost);
                }

                sentPost.IncrementAttempts();

                bool success = await chunkPublisher.TryPublishPostToSubscriberAsync(post, postContent, subscription, sentPost, cancellationToken);

                if (!success)
                {
                    RequiresRetry = true;
                }
            }
            catch (Exception ex)
            {
                dbContext.PostSendFailures.Add(PostSendFailure.CreateInternalError(post.Id, subscription.Id));
                Logger.LogCritical(ex, "Unhandled exception while publishing post: PostId={postId} ; Type={type}", post.Id, subscription.Id);
                RequiresRetry = true;
            }
        }

        await dbContext.SaveChangesAsync(discardConcurrentDeletedEntries: true, cancellationToken: cancellationToken);
    }

    private async Task<IEnumerable<TSubsbcription[]>> GetTargetSubscriptionChunks(PostId postId, DateTimeOffset postCreatedAt, ShareId shareId, CancellationToken cancellation)
    {
        var subscriptions = await DbContext.Subscriptions
                       .AsNoTracking()
                       .Where(x => x.Type == Publisher.Type)
                       .Where(x => x.ShareId == shareId && postCreatedAt >= x.CreatedAt)
                       .Where(x => !x.SentPosts!
                           .Any(x => x.PostId == postId && x.IsFinal))
                       .ToArrayAsync(cancellationToken: cancellation);

        return subscriptions
            .Select(x => (TSubsbcription)x)
            .Chunk(Publisher.ChunkSize);
    }
}

