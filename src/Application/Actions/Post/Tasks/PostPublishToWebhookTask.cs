using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeShare.Application.Common;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Tasks;
public class PostPublishToWebhookTask
{
    private const int ChunkSize = 50;
    private const int MaxRetries = 4;

    public class Command : IRequest
    {
        public PostId PostId { get; }

        public Command(PostId postId)
        {
            PostId = postId;
        }
    }

    public enum Status : byte
    {
        Success,
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly IShareContext DbContext;
        private readonly ILogger Logger;
        private readonly IWebhookClient WebhookClient;
        private readonly IPostStorage PostStorage;
        private readonly IServiceProvider Provider;
        private readonly IDispatcher Dispatcher;

        public bool AllDelivered = true;

        public Handler(IShareContext dbContext, ILogger<PostPublishToWebhookTask> logger, IWebhookClient webhookClient,
            IPostStorage postStorage, IServiceProvider provider, IDispatcher dispatcher)
        {
            DbContext = dbContext;
            Logger = logger;
            WebhookClient = webhookClient;
            PostStorage = postStorage;
            Provider = provider;
            Dispatcher = dispatcher;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var post = await DbContext.Posts
                .AsNoTracking()
                .Where(x => x.Id == request.PostId)
                .SingleOrDefaultAsync(cancellationToken);

            if (post is null)
            {
                Logger.LogError("Error while publishing post to webhooks. Post not found: PostId={postId}", request.PostId);
                return Unit.Value;
            }

            var content = await PostStorage.LoadAsync(post.Id, cancellationToken);

            if (content is null)
            {
                throw new InvalidOperationException($"Loading post content failed: PostId={post.Id}");
            }

            var options = new ParallelOptions()
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = 4,
            };

            await Parallel.ForEachAsync(await GetTargetSubscriptionChunks(request.PostId, post.CreatedAt, post.ShareId, cancellationToken), options,
                (subscribers, cancellationToken) => PublishPostChunkToSubscribersAsync(post, content, subscribers, cancellationToken));

            if (!AllDelivered)
            {
                Dispatcher.Schedule(request, $"Retry Publish Post To Webhook: {request.PostId}", TimeSpan.FromSeconds(30));
            }

            return Unit.Value;
        }

        private async ValueTask PublishPostChunkToSubscribersAsync(Post post, PostContent content, WebhookSubscription[] subscriptions, CancellationToken cancellationToken)
        {
            var dbContext = Provider.GetRequiredService<IShareContext>();

            foreach (var subscription in subscriptions)
            {
                var sentPost = await DbContext.SentPosts
                    .Where(x => x.SubscriptionId == subscription.Id)
                    .Where(x => x.PostId == post.Id)
                    .SingleOrDefaultAsync(cancellationToken);

                if (sentPost is null)
                {
                    sentPost = SentPost.Create(post.Id, subscription.Id);
                    DbContext.SentPosts.Add(sentPost);
                }

                sentPost.IncrementAttempts();

                bool success = await WebhookClient.TrySendPostAsync(subscription.TargetUrl, post, content, cancellationToken);

                if (success)
                {
                    sentPost.SetReceived();
                }
                else
                {
                    AllDelivered = false;
                }
            }

            //Ignore errors caused by subscriptions being removed during the execution
            var saveResult = await dbContext.SaveChangesAsync(discardConcurrentDeletedEntries: true, cancellationToken: cancellationToken);

            if (saveResult.Status != DbStatus.Success)
            {
                throw new Exception("Failed saving results of publish to database!");
            }
        }

        private async Task<IEnumerable<WebhookSubscription[]>> GetTargetSubscriptionChunks(PostId postId, DateTimeOffset postCreatedAt, ShareId shareId, CancellationToken cancellation)
        {
            var subscriptions = await DbContext.WebhookSubscriptions
                           .AsNoTracking()
                           .Where(x => x.ShareId == shareId && postCreatedAt >= x.CreatedAt)
                           .Where(x => !x.SentPosts!
                               .Where(x => x.PostId == postId)
                               .Where(x => x.Received || x.Attempts > MaxRetries)
                               .Any())
                           .ToArrayAsync(cancellationToken: cancellation);

            return subscriptions.Chunk(ChunkSize);
        }
    }
}

