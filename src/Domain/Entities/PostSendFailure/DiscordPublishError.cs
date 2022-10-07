namespace WeShare.Domain.Entities;
public enum DiscordPublishError
{
    DiscordUnresponsive,
    MissingRecipient,
    ChannelInaccessible,
    RateLimitHit,
}

