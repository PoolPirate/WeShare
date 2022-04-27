using Common.Services;
using Microsoft.Extensions.Logging;
using WeShare.Application.Services;
using WeShare.Domain.Entities;
using WeShare.Domain.Enums;
using WeShare.Infrastructure.Options;

namespace WeShare.Infrastructure.Services;
public class CallbackService : Singleton, ICallbackService
{
    [Inject]
    private readonly ISecretService SecretService;

    [Inject]
    private readonly CallbackOptions CallbackOptions;

    public Callback Create(UserId userId, CallbackType type)
    {
        var callbackSecret = SecretService.GenerateCallbackSecret();
        var callback = Callback.Create(callbackSecret, type, userId);
        Logger.LogInformation("Created Callback: Type={@Type} ; UserId={@UserId}", type, userId);
        return callback;
    }

    public bool Validate(Callback? callback) => callback is not null &&
            callback.SuccessfullySentAt >= GetCutoffTime(callback.Type);

    private DateTimeOffset GetCutoffTime(CallbackType type) => type switch
    {
        CallbackType.EmailVerification => DateTimeOffset.UtcNow - CallbackOptions.VerificationExpiration,
        CallbackType.PasswordReset => DateTimeOffset.UtcNow - CallbackOptions.PasswordResetExpiration,
        _ => throw new ArgumentException($"Invalid {nameof(CallbackType)}", nameof(type)),
    };
}
