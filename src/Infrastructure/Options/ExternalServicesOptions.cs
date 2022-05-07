using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace WeShare.Infrastructure.Options;
public class ExternalServicesOptions : Option
{
    public ulong WeShareDiscordServerId { get; set; }
    [Required]
    public string WeShareDiscordBotToken { get; set; }
}

