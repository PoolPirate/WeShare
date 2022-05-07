namespace WeShare.Domain.Entities;
public class DiscordPostSendFailure : PostSendFailure
{
    public DiscordPublishError PublishError { get; set; }

    public static DiscordPostSendFailure Create(PostId postId, SubscriptionId subscriptionId, DiscordPublishError publishError)
        => new DiscordPostSendFailure(postId, subscriptionId, publishError);

    protected DiscordPostSendFailure(PostId postId, SubscriptionId subscriptionId, DiscordPublishError publishError)
        : base(postId, subscriptionId,
            publishError == DiscordPublishError.ChannelInaccessible || publishError == DiscordPublishError.MissingRecipient,
            PostSendFailureType.MessagerDiscord)
    {
        PublishError = publishError;
    }
}

