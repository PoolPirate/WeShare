using Common.Services;
using Microsoft.EntityFrameworkCore;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services.PostPublishers.Types;
public class DiscordPublisher : Scoped, ISusbcriptionPostPublisher<DiscordSubscription>
{
    public SubscriptionType Type => SubscriptionType.MessagerDiscord;
    public int ChunkSize => 10;
    public int DegreeOfParallelism => 1;

    [Inject]
    private readonly IDiscordClient DiscordClient = null!;
    [Inject]
    private readonly IShareContext DbContext = null!;

    public async Task<bool> TryPublishPostToSubscriberAsync(Post post, PostContent content, DiscordSubscription subscription, SentPost sentPost,
        CancellationToken cancellationToken)
    {
        var recipientsResponse = await DiscordClient.GetDMChannelRecipientsAsync(subscription.ChannelId, cancellationToken);

        if (!HandleRecipientsResponse(recipientsResponse.Status, sentPost))
        {
            return false;
        }

        bool hasRecipient = await DbContext.DiscordConnections
            .Where(x => x.UserId == subscription.UserId)
            .AnyAsync(x => recipientsResponse.Value!.Contains(x.DiscordId), cancellationToken: cancellationToken);

        if (!hasRecipient)
        {
            DbContext.PostSendFailures.Add(DiscordPostSendFailure.Create(post.Id, subscription.Id, DiscordPublishError.MissingRecipient));
            return false;
        }

        var sendResponse = await DiscordClient.SendMessageAsync(subscription.ChannelId, $"New Post: https://we-share-live.de/post/{post.Id}", cancellationToken);
        if (!HandleMessageSendResponse(sendResponse.Status, sentPost))
        {
            return false;
        }

        sentPost.SetReceived();
        return true;
    }

    private bool HandleRecipientsResponse(DiscordStatus status, SentPost sentPost)
    {
        switch (status)
        {
            case DiscordStatus.Success:
                return true;
            case DiscordStatus.Forbidden:
                DbContext.PostSendFailures.Add(DiscordPostSendFailure.Create(sentPost.PostId, sentPost.SubscriptionId, DiscordPublishError.ChannelInaccessible));
                sentPost.SetFailed();
                return false;
            case DiscordStatus.RateLimited:
                DbContext.PostSendFailures.Add(PostSendFailure.CreateInternalError(sentPost.PostId, sentPost.SubscriptionId));
                return false;
            case DiscordStatus.Unavailable:
                DbContext.PostSendFailures.Add(DiscordPostSendFailure.Create(sentPost.PostId, sentPost.SubscriptionId, DiscordPublishError.DiscordUnresponsive));
                return false;
            default:
                throw new InvalidOperationException();
        }
    }

    private bool HandleMessageSendResponse(DiscordStatus status, SentPost sentPost)
    {
        switch (status)
        {
            case DiscordStatus.Success:
                return true;
            case DiscordStatus.Forbidden:
                DbContext.PostSendFailures.Add(DiscordPostSendFailure.Create(sentPost.PostId, sentPost.SubscriptionId, DiscordPublishError.ChannelInaccessible));
                sentPost.SetFailed();
                return false;
            case DiscordStatus.RateLimited:
                DbContext.PostSendFailures.Add(PostSendFailure.CreateInternalError(sentPost.PostId, sentPost.SubscriptionId));
                return false;
            case DiscordStatus.Unavailable:
                DbContext.PostSendFailures.Add(DiscordPostSendFailure.Create(sentPost.PostId, sentPost.SubscriptionId, DiscordPublishError.DiscordUnresponsive));
                return false;
            default:
                throw new InvalidOperationException();
        }
    }
}

