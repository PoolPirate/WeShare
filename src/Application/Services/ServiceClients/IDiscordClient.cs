using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IDiscordClient : IService
{
    Task<DiscordId?> LoadDiscordUserIdAsync(string accessToken, CancellationToken cancellationToken);
    Task<bool> AddUserToWeShareGuild(string accessToken, DiscordId userId, CancellationToken cancellationToken);

    Task SendWelcomeMessage(DiscordId userId);
}

