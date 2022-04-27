using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace WeShare.Infrastructure.Options;

[SectionName("JWT")]
public class JwtOptions : Option
{
    [Required]
    [MinLength(16)]
    public string JwtKey { get; set; }
    [Required]
    public string JwtIssuer { get; set; } = "WeShare";
    [Range(1, Int32.MaxValue)]
    public int JwtExpirationSeconds { get; set; } = 3600;
}
