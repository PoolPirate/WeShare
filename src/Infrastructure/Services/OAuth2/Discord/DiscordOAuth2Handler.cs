using Common.Services;
using System.Net.Http.Json;
using WeShare.Application.Actions.Commands;
using WeShare.Infrastructure.Options;

namespace WeShare.Infrastructure.Services.OAuth2;
public class DiscordOAuth2Handler : Singleton
{
    [Inject]
    private readonly HttpClient HttpClient;

    [Inject]
    private readonly OAuth2Options OAuth2Options;
    [Inject]
    private readonly DiscordOAuth2Options DiscordOAuth2Options;

    public async Task<OAuth2TokenResponse?> GetAccessTokenAsync(string code, CancellationToken cancellationToken)
    {
        var response = await HttpClient.PostAsync(DiscordRoutes.TokenEndpoint,
            new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["client_id"] = DiscordOAuth2Options.ClientId,
                ["client_secret"] = DiscordOAuth2Options.ClientSecret,
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = OAuth2Options.RedirectUri.ToString(),
            }),
            cancellationToken);

        return !response.IsSuccessStatusCode
            ? null
            : await response.Content.ReadFromJsonAsync<OAuth2TokenResponse>(cancellationToken: cancellationToken);
    }
}

