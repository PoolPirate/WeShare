using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class UserCreateAction
{
    public class Command : IRequest<Result>
    {
        public Username Username { get; }

        public Nickname Nickname { get; }

        [EmailAddress]
        [MaxLength(DomainConstraints.EmailLengthMaximum)]
        [Required]
        public string Email { get; }

        public PlainTextPassword Password { get; }

        public Command(Username username, Nickname nickname, string email, PlainTextPassword password)
        {
            Username = username;
            Nickname = nickname;
            Email = email.ToUpper();
            Password = password;
        }
    }

    public enum Status
    {
        Success,
        EmailTaken,
        UsernameTaken,
    }

    public record struct Result(Status Status);

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
            string passwordHash = SecretService.HashPassword(request.Password);
            var user = User.Create(request.Username, request.Email, passwordHash, request.Nickname);
            DbContext.Users.Add(user);

            using var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);

            var saveResult = await DbContext.SaveChangesAsync(
                allowedStatuses: DbStatus.DuplicateIndex, transactionScope: transactionScope, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.DuplicateIndex => new Result(
                    saveResult.IsConflictingIndex(typeof(User), nameof(User.Username))
                        ? Status.UsernameTaken
                        :
                    saveResult.IsConflictingIndex(typeof(User), nameof(User.Email))
                        ? Status.EmailTaken
                        :
                    throw new UnhandledIndexConflictException(saveResult)),
                _ => throw new UnhandledDbStatusException(saveResult)
            };
        }
    }
}
