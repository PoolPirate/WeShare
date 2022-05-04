using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace WeShare.Infrastructure.Options;
public class DiscordOAuth2Options : Option
{
    [Required]
    public string ClientId { get; set; }
    [Required]
    public string ClientSecret { get; set; }
}

