using System.Text.Json.Serialization;

namespace WeShare.Application.Actions.Commands;
public class OAuth2TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = null!;

    [JsonPropertyName("scope")]
    public string Scope { get; set; } = null!;

    [JsonConstructor]
    public OAuth2TokenResponse()
    {
    }
}

