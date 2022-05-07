namespace WeShare.Domain.Entities;
public class DiscordSubscription : Subscription
{
    public DiscordId ChannelId { get; set; }

    public static DiscordSubscription Create(SubscriptionName name, UserId userId, ShareId shareId, DiscordId channelId)
        => new DiscordSubscription(name, userId, shareId, channelId);

    protected DiscordSubscription(SubscriptionName name, UserId userId, ShareId shareId, DiscordId channelId)
        : base(SubscriptionType.MessagerDiscord, name, userId, shareId)
    {
        ChannelId = channelId;
    }
}
