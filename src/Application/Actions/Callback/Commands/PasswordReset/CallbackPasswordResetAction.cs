using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class CallbackPasswordResetAction
{
    public class Command : IRequest<Result>
    {
        public CallbackSecret CallbackSecret { get; }

        public PlainTextPassword Password { get; }

        public Command(CallbackSecret callbackSecret, PlainTextPassword password)
        {
            CallbackSecret = callbackSecret;
            Password = password;
        }
    }

    public enum Status : byte
    {
        Success,
        CallbackNotFound,
        InvalidCallback,
        UserNotFound,
    }

    public record Result(Status Status);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly ICallbackService CallbackService;
        private readonly ISecretService SecretService;

        public Handler(IShareContext dbContext, ICallbackService callbackService, ISecretService secretService)
        {
            DbContext = dbContext;
            CallbackService = callbackService;
            SecretService = secretService;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var callback = await DbContext.Callbacks
                .Where(x => x.Secret == request.CallbackSecret)
                .FirstOrDefaultAsync(cancellationToken);

            if (callback is null)
            {
                return new Result(Status.CallbackNotFound);
            }
            if (!CallbackService.Validate(callback))
            {
                return new Result(Status.InvalidCallback);
            }

            var user = await DbContext.Users
                .Where(x => x.Id == callback.OwnerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                return new Result(Status.UserNotFound);
            }

            DbContext.Callbacks.Remove(callback);
            user.PasswordHash = SecretService.HashPassword(request.Password);

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.ConcurrencyEntryDeleted, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.ConcurrencyEntryDeleted => new Result(Status.CallbackNotFound),
                _ => throw new UnhandledDbStatusException(saveResult)
            };
        }
    }
}

