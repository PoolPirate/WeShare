using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Services;
public static class DiscordRoutes
{
    public const string TokenEndpoint = "https://discord.com/api/oauth2/token";
    public const string CurrentUserInformationEndpoint = "https://discord.com/api/users/@me";

    public static string AddUserToGuildEndpoint(DiscordId guildId, DiscordId userId)
        => $"https://discord.com/api/guilds/{guildId}/members/{userId}";

    public const string CreateDMChannel = "https://discord.com/api/users/@me/channels";
    public static string CreateMessage(DiscordId channelId)
        => $"https://discord.com/api/channels/{channelId}/messages";

    public static string GetChannel(DiscordId channelId)
        => $"https://discord.com/api/channels/{channelId}";
}

