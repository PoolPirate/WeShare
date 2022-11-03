using Common.Services;
using WeShare.Application.Entities;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IDiscordClient : IService
{
    Task<DiscordResponse<DiscordId>> LoadDiscordUserIdAsync(string accessToken, CancellationToken cancellationToken);
    Task<DiscordResponse> AddUserToWeShareGuild(string accessToken, DiscordId userId, CancellationToken cancellationToken);

    Task<DiscordResponse<DiscordId>> GetDMChannelId(DiscordId userId, CancellationToken cancellationToken);
    Task<DiscordResponse<IList<DiscordId>>> GetDMChannelRecipientsAsync(DiscordId channelId, CancellationToken cancellationToken);
    Task<DiscordResponse> SendMessageAsync(DiscordId channelId, DiscordEmbed content, CancellationToken cancellationToken);
}

