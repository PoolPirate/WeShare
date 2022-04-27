using Common.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WeShare.WebAPI.Options;

[SectionName("Binding")]
public sealed class BindingOptions : Option
{
    [Range(1024, UInt16.MaxValue)]
    public ushort ApplicationPort { get; init; } = 8765;
    public IPAddress BindAddress { get; set; } = IPAddress.Any;
}
