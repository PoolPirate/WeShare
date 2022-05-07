using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services.PostPublishers.Types;
public class WebhookPublisher : Scoped, ISusbcriptionPostPublisher<WebhookSubscription>
{
    public SubscriptionType Type => SubscriptionType.Webhook;
    public int ChunkSize => 20;
    public int DegreeOfParallelism => 4;

    [Inject]
    private readonly IShareContext DbContext = null!;
    [Inject]
    private readonly IWebhookClient WebhookClient = null!;

    public async Task<bool> TryPublishPostToSubscriberAsync(Post post, PostContent content, WebhookSubscription subscription, SentPost sentPost, 
        CancellationToken cancellationToken)
    {
        var (success, statusCode, latency) = await WebhookClient.TrySendPostAsync(subscription.TargetUrl, post, content, cancellationToken);

        if (success)
        {
            sentPost.SetReceived();
            return true;
        }

        DbContext.PostSendFailures.Add(WebhookPostSendFailure.Create(post.Id, subscription.Id, statusCode, latency));
        return false;
    }
}

