using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class UserPasswordUpdateAction
{
    public class Command : IRequest<Result>
    {
        public UserId UserId { get; }

        public PlainTextPassword Password { get; }

        public Command(UserId userId, PlainTextPassword password)
        {
            UserId = userId;
            Password = password;
        }
    }

    public enum Status : byte
    {
        Success,
        UserNotFound,
    }

    public record Result(Status Status);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly ISecretService SecretService;

        public Handler(IShareContext dbContext, ISecretService secretService)
        {
            DbContext = dbContext;
            SecretService = secretService;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await DbContext.Users
                .Where(x => x.Id == request.UserId)
                .SingleOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                return new Result(Status.UserNotFound);
            }

            user.PasswordHash = SecretService.HashPassword(request.Password);

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.ConcurrencyEntryDeleted, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.ConcurrencyEntryDeleted => new Result(Status.UserNotFound),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

