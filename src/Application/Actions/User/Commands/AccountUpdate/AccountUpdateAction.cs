using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class AccountUpdateAction
{
    public class Command : IRequest<Result>
    {
        public UserId UserId { get; }

        public Username? NewUsername { get; }

        [MaxLength(DomainConstraints.EmailLengthMaximum)]
        [EmailAddress]
        public string? Email { get; }

        public Command(UserId userId, string? username, string? email)
        {
            UserId = userId;
            NewUsername = username is not null ? Username.From(username) : null;
            Email = email;
        }
    }

    public enum Status : byte
    {
        Success,
        UserNotFound,
        UsernameTaken,
    }

    public record Result(Status Status);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IAuthorizer Authorizer;

        public Handler(IShareContext dbContext, IAuthorizer authorizer)
        {
            DbContext = dbContext;
            Authorizer = authorizer;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await Authorizer.EnsureAuthorizationAsync(request.UserId, UserCommandOperation.UpdateAccount, cancellationToken);

            var user = await DbContext.Users
                .Where(x => x.Id == request.UserId)
                .SingleOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                return new Result(Status.UserNotFound);
            }

            ApplyUpdates(user, request);

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.ConcurrencyEntryDeleted | DbStatus.DuplicateIndex, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.ConcurrencyEntryDeleted => new Result(Status.UserNotFound),
                DbStatus.DuplicateIndex => new Result(
                    saveResult.IsConflictingIndex(typeof(User), nameof(User.Username))
                        ? Status.UsernameTaken
                        : throw new UnhandledIndexConflictException(saveResult)
                    ),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }

        private static void ApplyUpdates(User user, Command request)
        {
            if (request.NewUsername.HasValue)
            {
                user.Username = request.NewUsername.Value;
            }
            if (request.Email is not null)
            {
                user.UpdateEmail(request.Email);
            }
        }
    }
}

