using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace WeShare.Infrastructure.Options;

[SectionName("Database")]
public class DatabaseOptions : Option
{
    [Required]
    public string AppConnectionString { get; init; }

    [Required]
    public string HangfireConnectionString { get; init; }
}
