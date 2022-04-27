using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain;
using WeShare.Domain.Entities;
using WeShare.Domain.Enums;

namespace WeShare.Application.Actions.Commands;
public class UserRequestPasswordResetAction
{
    public class Command : IRequest<Result>
    {
        [EmailAddress]
        [MaxLength(DomainConstraints.EmailLengthMaximum)]
        [Required]
        public string Email { get; }

        public Command(string email)
        {
            Email = email.ToUpper();
        }
    }

    public enum Status : byte
    {
        Success,
        EmailNotRegistered,
        LastRequestStillValid,
    }

    public record Result(Status Status);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IAuthorizer Authorizer;
        private readonly ICallbackService CallbackService;

        public Handler(IShareContext dbContext, IAuthorizer authorizer, ICallbackService callbackService)
        {
            DbContext = dbContext;
            Authorizer = authorizer;
            CallbackService = callbackService;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var userIdData = await DbContext.Users
                .Where(x => x.Email == request.Email)
                .Select(x => new { UserId = x.Id })
                .SingleOrDefaultAsync(cancellationToken);

            if (userIdData is null)
            {
                return new Result(Status.EmailNotRegistered);
            }

            var userId = userIdData.UserId;
            await Authorizer.EnsureAuthorizationAsync(userId, UserCommandOperation.RequestPasswordReset, cancellationToken);

            var callback = CallbackService.Create(userId, CallbackType.PasswordReset);
            DbContext.Callbacks.Add(callback);

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.DuplicateIndex, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.DuplicateIndex => new Result(
                    saveResult.IsConflictingIndex(typeof(Callback), nameof(Callback.Type), nameof(Callback.OwnerId))
                        ? Status.LastRequestStillValid
                        :
                    throw new UnhandledIndexConflictException(saveResult)),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

