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
    private const int ChunksSize = 50;

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

        public Handler(IShareContext dbContext, ILogger<PostPublishToWebhookTask> logger, IWebhookClient webhookClient, 
            IPostStorage postStorage, IServiceProvider provider)
        {
            DbContext = dbContext;
            Logger = logger;
            WebhookClient = webhookClient;
            PostStorage = postStorage;
            Provider = provider;
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

            var options = new ParallelOptions()
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = 4,
            };

            await Parallel.ForEachAsync(GetTargetSubscriptionChunks(request.PostId, post.CreatedAt, post.ShareId), options,
                (subscribers, cancellationToken) => PublishPostChunkToSubscribersAsync(post, content, subscribers, cancellationToken));

            return Unit.Value;
        }

        private async ValueTask PublishPostChunkToSubscribersAsync(Post post, PostContent content, Subscription[] subscriptions, CancellationToken cancellationToken)
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

                bool success = await WebhookClient.TrySendPostAsync(post, content, subscription, cancellationToken);

                if (success)
                {
                    sentPost.SetReceived();
                }
            }

            //Ignore errors caused by subscriptions being removed during the execution
            var saveResult = await dbContext.SaveChangesAsync(discardConcurrentDeletedEntries: true, cancellationToken: cancellationToken);

            if (saveResult.Status != DbStatus.Success)
            {
                throw new Exception("Failed saving results of publish to database!");
            }
        }

        private IAsyncEnumerable<Subscription[]> GetTargetSubscriptionChunks(PostId postId, DateTimeOffset postCreatedAt, ShareId shareId) 
            => DbContext.Subscriptions
                .AsNoTracking()
                .Where(x => x.ShareId == shareId)
                .Where(x => x.CreatedAt >= postCreatedAt)
                .Where(x => !x.SentPosts!
                    .Where(x => x.PostId == postId)
                    .Where(x => x.Received)
                    .Any())
                .Chunk(ChunksSize)
                .AsAsyncEnumerable();
    }
}

