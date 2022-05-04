using System;
using System.Text.Json.Serialization;

namespace WeShare.Infrastructure.Services.OAuth2;
public class DiscordUser
{
    [JsonPropertyName("id")]
    public ulong Id { get; set; }

    [JsonConstructor]
    public DiscordUser()
    {
    }
}

