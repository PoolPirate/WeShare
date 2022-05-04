using Common.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WeShare.Application.Services;
using WeShare.Domain.Entities;
using WeShare.Infrastructure.Options;

namespace WeShare.Infrastructure.Services;
public class DiscordClient : Singleton, IDiscordClient
{
    [Inject]
    private readonly HttpClient HttpClient;

    [Inject]
    private readonly ExternalServicesOptions ExternalServicesOptions;

    public async Task<bool> AddUserToWeShareGuild(string accessToken, DiscordId userId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, DiscordRoutes.AddUserToGuildEndpoint(
            DiscordId.From(ExternalServicesOptions.WeShareDiscordServerId), userId));

        request.Headers.Authorization = new AuthenticationHeaderValue("Bot", ExternalServicesOptions.WeShareDiscordBotToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        request.Content = JsonContent.Create(new Dictionary<string, string>()
        {
            ["access_token"] = accessToken,
        });

        var response = await HttpClient.SendAsync(request, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<DiscordId?> LoadDiscordUserIdAsync(string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, DiscordRoutes.UserInformationEndpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await HttpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var user = await response.Content.ReadFromJsonAsync<DiscordUser>(cancellationToken: cancellationToken);

        return user is null
            ? null
            : DiscordId.From(user.Id);
    }

    public async Task SendWelcomeMessage(DiscordId userId)
    {
        var channelId = await GetDMChannelIdAsync(userId);
        await SendMessageAsync(channelId, "Welcome!");
    }

    private async Task<DiscordId> GetDMChannelIdAsync(DiscordId userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, DiscordRoutes.CreateDMChannel);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bot", ExternalServicesOptions.WeShareDiscordBotToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        request.Content = JsonContent.Create(new Dictionary<string, string>()
        {
            ["recipient_id"] = userId.ToString(),
        });

        var response = await HttpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var channel = await response.Content.ReadFromJsonAsync<DiscordChannel>();

        return channel is null 
            ? throw new HttpRequestException("Missing channel response") 
            : channel.Id;
    }

    private async Task SendMessageAsync(DiscordId channelId, string content)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, DiscordRoutes.CreateMessage(channelId));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bot", ExternalServicesOptions.WeShareDiscordBotToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        request.Content = JsonContent.Create(new Dictionary<string, string>()
        {
            ["content"] = content,
        });

        var response = await HttpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}

