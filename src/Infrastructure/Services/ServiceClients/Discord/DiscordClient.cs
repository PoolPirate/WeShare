using Common.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WeShare.Application.Entities;
using WeShare.Application.Services;
using WeShare.Domain.Entities;
using WeShare.Infrastructure.Extensions;
using WeShare.Infrastructure.Options;

namespace WeShare.Infrastructure.Services;
public class DiscordClient : Singleton, IDiscordClient
{
    [Inject]
    private readonly HttpClient HttpClient;
    [Inject]
    private readonly ExternalServicesOptions ExternalServicesOptions;

    public async Task<DiscordResponse> AddUserToWeShareGuild(string accessToken, DiscordId userId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, DiscordRoutes.AddUserToGuildEndpoint(
            DiscordId.From(ExternalServicesOptions.WeShareDiscordServerId), userId));

        request.Headers.Authorization = new AuthenticationHeaderValue("Bot", ExternalServicesOptions.WeShareDiscordBotToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        request.Content = JsonContent.Create(new Dictionary<string, string>()
        {
            ["access_token"] = accessToken,
        });

        var (received, response) = await HttpClient.SendSafeAsync(request, cancellationToken);

        return received
            ? DiscordResponse.FromHttpResponse(response!)
            : DiscordResponse.FromTimeout();
    }

    public async Task<DiscordResponse<DiscordId>> LoadDiscordUserIdAsync(string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, DiscordRoutes.CurrentUserInformationEndpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var (received, response) = await HttpClient.SendSafeAsync(request, cancellationToken);

        return received
            ? await DiscordResponse<DiscordId>.FromHttpResponseAsync<DiscordUser>(response!, x => x.Id)
            : DiscordResponse<DiscordId>.FromTimeout();
    }

    public async Task<DiscordResponse<IList<DiscordId>>> GetDMChannelRecipientsAsync(DiscordId channelId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, DiscordRoutes.GetChannel(channelId));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bot", ExternalServicesOptions.WeShareDiscordBotToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var (received, response) = await HttpClient.SendSafeAsync(request, cancellationToken);

        return received
            ? await DiscordResponse<IList<DiscordId>>.FromHttpResponseAsync<DiscordChannel>(response!, x => x.Recipients?.Select(x => x.Id)?.ToList() ?? throw new Exception("Wrong channel type!"))
            : DiscordResponse<IList<DiscordId>>.FromTimeout();
    }

    public async Task<DiscordResponse> SendMessageAsync(DiscordId channelId, DiscordEmbed content, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, DiscordRoutes.CreateMessage(channelId));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bot", ExternalServicesOptions.WeShareDiscordBotToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        request.Content = JsonContent.Create(new Dictionary<string, object>()
        {
            ["embed"] = content,
        });

        var (received, response) = await HttpClient.SendSafeAsync(request, cancellationToken);

        return received
            ? DiscordResponse.FromHttpResponse(response!)
            : DiscordResponse.FromTimeout();
    }

    public async Task<DiscordResponse<DiscordId>> GetDMChannelId(DiscordId userId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, DiscordRoutes.CreateDMChannel);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bot", ExternalServicesOptions.WeShareDiscordBotToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        request.Content = JsonContent.Create(new Dictionary<string, string>()
        {
            ["recipient_id"] = userId.ToString(),
        });

        var (received, response) = await HttpClient.SendSafeAsync(request, cancellationToken);

        return received
            ? await DiscordResponse<DiscordId>.FromHttpResponseAsync<DiscordChannel>(response!, x => x.Id)
            : DiscordResponse<DiscordId>.FromTimeout();
    }
}
