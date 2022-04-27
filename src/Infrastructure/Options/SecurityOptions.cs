using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace WeShare.Infrastructure.Options;

[SectionName("Security")]
public class SecurityOptions : Option
{
    [Range(4, 32)]
    public int HashWorkFactor { get; set; }

    [Range(4, 1024)]
    public short CallbackSecretLength { get; set; } = 12;

    [Range(4, 1024)]
    public short ShareSecretLength { get; set; } = 24;

    public SecurityOptions()
    {
    }
}
