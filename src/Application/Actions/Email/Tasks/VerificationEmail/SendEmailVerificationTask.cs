using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WeShare.Application.Services;
using WeShare.Domain.Entities;
using WeShare.Domain.Enums;

namespace WeShare.Application.Actions.Tasks;
public class SendEmailVerificationTask
{
    public class Command : IRequest
    {
        public CallbackId CallbackId;

        public Command(CallbackId callbackId)
        {
            CallbackId = callbackId;
        }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly IShareContext DbContext;
        private readonly IEmailSender EmailSender;
        private readonly ILogger Logger;

        public Handler(IShareContext dbContext, IEmailSender emailSender, ILogger<SendEmailVerificationTask> logger)
        {
            DbContext = dbContext;
            EmailSender = emailSender;
            Logger = logger;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var callback = await DbContext.Callbacks
                .Where(x => x.Id == request.CallbackId)
                .SingleOrDefaultAsync(cancellationToken);

            if (callback is null)
            {
                Logger.LogCritical("Failed sending email, Callback not found: CallbackId={callbackId}", request.CallbackId);
                return Unit.Value;
            }
            if (callback.SuccessfullySentAt is not null)
            {
                Logger.LogWarning("Not sending email, Already sucessfully sent: CallbackId={callbackId}", request.CallbackId);
                return Unit.Value;
            }
            if (callback.Type != CallbackType.EmailVerification)
            {
                throw new InvalidOperationException($"Invalid Callback Type: CallbackId={request.CallbackId}");
            }

            var userData = await DbContext.Users
                .Where(x => x.Id == callback.OwnerId)
                .Select(x => new { x.Username, x.Nickname, x.Email })
                .SingleOrDefaultAsync(cancellationToken);

            if (userData is null)
            {
                Logger.LogWarning("Failed sending email, User not found: CallbackId={callbackId} ; UserId={userId}", request.CallbackId, callback.OwnerId);
                return Unit.Value;
            }

            await EmailSender.SendVerificationEmail(userData.Username, userData.Nickname, userData.Email, callback.Secret);
            callback.SuccessfullySentAt = DateTimeOffset.UtcNow;

            Logger.LogInformation("Sucessfully sent email: CallbackId={callbackId} ; UserId={userId}", request.CallbackId, callback.OwnerId);

            await DbContext.SaveChangesAsync(cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
