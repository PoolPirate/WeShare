using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace WeShare.Infrastructure.Options;

[SectionName("SendGrid")]
public class SendGridOptions : Option
{
    [Required]
    [EmailAddress]
    public string SenderAddress { get; set; } = "Admin@WeShare.de";

    [Required]
    public string SenderName { get; set; } = "WeShare";

    [Required]
    public string ApiKey { get; set; }
}
