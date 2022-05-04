using Common.Services;
using WeShare.Application.Services;
using WeShare.Domain.Entities;
using WeShare.Infrastructure.Options;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace WeShare.Infrastructure.Services.OAuth2;
public class DiscordOAuth2Handler : Singleton, IDiscordClient
{
    [Inject]
    private readonly HttpClient HttpClient;

    [Inject]
    private readonly OAuth2Options OAuth2Options;
    [Inject]
    private readonly DiscordOAuth2Options DiscordOAuth2Options;

    public async Task<string?> GetAccessTokenAsync(string code, CancellationToken cancellationToken)
    {
        var response = await HttpClient.PostAsync(DiscordOAuth2.TokenEndpoint,
            new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["client_id"] = DiscordOAuth2Options.ClientId,
                ["client_secret"] = DiscordOAuth2Options.ClientSecret,
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = OAuth2Options.RedirectUri.ToString(),
            }),
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var oauthResponse = await response.Content.ReadFromJsonAsync<OAuth2TokenResponse>(cancellationToken: cancellationToken);
        return oauthResponse?.AccessToken;
    }

    public async Task<DiscordId?> LoadDiscordIdAsync(string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, DiscordOAuth2.UserInformationEndpoint);
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
}

