using System.Text.Json.Serialization;

namespace WeShare.Infrastructure.Services.OAuth2;
public class OAuth2TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonConstructor]
    public OAuth2TokenResponse()
    {
    }
}

