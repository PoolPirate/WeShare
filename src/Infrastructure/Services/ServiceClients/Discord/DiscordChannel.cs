using System.Text.Json.Serialization;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Services;
public class DiscordChannel
{
    [JsonPropertyName("id")]
    public DiscordId Id { get; set; }

    [JsonPropertyName("recipients")]
    public IList<DiscordUser>? Recipients { get; set; }

    [JsonConstructor]
    public DiscordChannel()
    {
    }
}

