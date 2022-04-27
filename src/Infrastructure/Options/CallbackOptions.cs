using Common.Configuration;

namespace WeShare.Infrastructure.Options;
[SectionName("Callback")]
public class CallbackOptions : Option
{
    public TimeSpan VerificationExpiration { get; set; } = TimeSpan.FromHours(2);
    public TimeSpan PasswordResetExpiration { get; set; } = TimeSpan.FromMinutes(15);
}
