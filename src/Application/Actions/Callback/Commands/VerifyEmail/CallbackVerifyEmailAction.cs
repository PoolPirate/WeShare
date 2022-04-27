using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class CallbackVerifyEmailAction
{
    public class Command : IRequest<Result>
    {
        public CallbackSecret CallbackSecret { get; }

        public Command(CallbackSecret callbackSecret)
        {
            CallbackSecret = callbackSecret;
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

        public Handler(IShareContext dbContext, ICallbackService callbackService)
        {
            DbContext = dbContext;
            CallbackService = callbackService;
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
            user.VerifyEmail();

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

