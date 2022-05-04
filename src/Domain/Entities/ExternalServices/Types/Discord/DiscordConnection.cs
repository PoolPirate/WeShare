namespace WeShare.Domain.Entities;
public class DiscordConnection : ServiceConnection
{
    public DiscordId DiscordId { get; init; }

    public static DiscordConnection Create(UserId userId, DiscordId discordId)
        => new DiscordConnection(userId, discordId);

    protected DiscordConnection(UserId userId, DiscordId discordId) 
        : base(userId, ServiceConnectionType.Discord)
    {
        DiscordId = discordId;
    }
}

