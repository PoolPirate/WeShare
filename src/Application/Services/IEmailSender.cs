using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;

public interface IEmailSender : IService
{
    public Task SendVerificationEmail(Username username, Nickname nickname, string emailAddress, CallbackSecret callbackSecret);
    public Task SendPasswordResetEmail(Username username, Nickname nickname, string emailAddress, CallbackSecret callbackSecret);
}
