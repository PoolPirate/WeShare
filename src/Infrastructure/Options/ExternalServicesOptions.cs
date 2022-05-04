using Common.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WeShare.Infrastructure.Options;
public class ExternalServicesOptions : Option
{
    public ulong WeShareDiscordServerId { get; set; }
    [Required]
    public string WeShareDiscordBotToken { get; set; }
}

