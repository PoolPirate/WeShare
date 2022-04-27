using Common.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using WeShare.Application.Services;
using WeShare.Domain.Entities;
using WeShare.Infrastructure.Options;

namespace WeShare.Infrastructure.Services;

public class EmailSender : Singleton, IEmailSender
{
    [Inject]
    private readonly ISendGridClient SendGridClient;

    [Inject]
    private readonly SendGridOptions EmailOptions;

    public async Task SendPasswordResetEmail(Username username, Nickname nickname, string emailAddress, CallbackSecret callbackSecret)
    {
        string content = $"You can reset the password <a href=\"https://localhost:44440/c/resetpassword/{callbackSecret}\">here</a>";

        var message = new SendGridMessage()
        {
            Subject = "Password Reset",
            HtmlContent = content
        };

        message.SetFrom(EmailOptions.SenderAddress, EmailOptions.SenderName);
        message.AddTo(emailAddress, username.Value);

        var sendResponse = await SendGridClient.SendEmailAsync(message);
        EnsureSuccessfulResponse(sendResponse);
    }

    public async Task SendVerificationEmail(Username username, Nickname nickname, string emailAddress, CallbackSecret callbackSecret)
    {
        string content = $"You can verify the account <a href=\"https://localhost:44440/c/verifyemail/{callbackSecret}\">here</a>";

        var message = new SendGridMessage()
        {
            Subject = "Account Verification",
            HtmlContent = content
        };

        message.SetFrom(EmailOptions.SenderAddress, EmailOptions.SenderName);
        message.AddTo(emailAddress, username.Value);

        var sendResponse = await SendGridClient.SendEmailAsync(message);
        EnsureSuccessfulResponse(sendResponse);
    }

    private static async void EnsureSuccessfulResponse(Response response)
    {
        if (!response.IsSuccessStatusCode)
        {
            string reason = await response.Body.ReadAsStringAsync();
            throw new HttpRequestException($"Sending email failed: StatusCode={response.StatusCode} Reason={reason}");
        }
    }
}
