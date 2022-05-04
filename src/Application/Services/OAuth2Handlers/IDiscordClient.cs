using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface IDiscordClient : IService
{
    Task<DiscordId?> LoadDiscordIdAsync(string accessToken, CancellationToken cancellationToken);
}

