using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace WeShare.Infrastructure.Options;

[SectionName("Security")]
public class SecurityOptions : Option
{
    [Range(4, 32)]
    public int HashWorkFactor { get; set; }

    public SecurityOptions()
    {
    }
}
