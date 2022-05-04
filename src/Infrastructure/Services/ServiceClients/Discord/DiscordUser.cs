using System;
using System.Text.Json.Serialization;

namespace WeShare.Infrastructure.Services;
public class DiscordUser
{
    [JsonPropertyName("id")]
    public ulong Id { get; set; }

    [JsonConstructor]
    public DiscordUser()
    {
    }
}

