using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace WeShare.Infrastructure.Options;
public class OAuth2Options : Option
{
    [Required]
    public Uri RedirectUri { get; set; }
}

