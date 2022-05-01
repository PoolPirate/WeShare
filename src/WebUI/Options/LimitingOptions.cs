using Common.Configuration;

namespace WeShare.WebAPI.Options;

public class LimitingOptions : Option
{
    public bool DisableRegister { get; set; } = false;
}
