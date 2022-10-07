using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace WeShare.WebAPI.Options;

[SectionName("Authorization")]
public class AuthorizationOptions : Option
{
    [Required]
    [MinLength(16)]
    public string HangfireCookieSecret = null!;
}
