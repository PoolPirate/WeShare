using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class ProfileUpdateAction
{
    public class Command : IRequest<Result>
    {
        public UserId UserId { get; }

        [MinLength(DomainConstraints.NicknameLengthMinimum)]
        [MaxLength(DomainConstraints.NicknameLengthMaximum)]
        public Nickname? Nickname { get; }

        public bool? LikesPublished { get; }

        public Command(UserId userId, Nickname? nickname, bool? likesPublished)
        {
            UserId = userId;
            Nickname = nickname;
            LikesPublished = likesPublished;
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
        private readonly IAuthorizer Authorizer;

        public Handler(IShareContext dbContext, IAuthorizer authorizer)
        {
            DbContext = dbContext;
            Authorizer = authorizer;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            await Authorizer.EnsureAuthorizationAsync(request.UserId, UserCommandOperation.UpdateProfile, cancellationToken);

            var user = await DbContext.Users
                .Where(x => x.Id == request.UserId)
                .SingleOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                return new Result(Status.UserNotFound);
            }

            ApplyUpdates(user, request);

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.ConcurrencyEntryDeleted, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.ConcurrencyEntryDeleted => new Result(Status.UserNotFound),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }

        private static void ApplyUpdates(User user, Command request)
        {
            if (request.Nickname.HasValue)
            {
                user.Nickname = request.Nickname.Value;
            }
            if (request.LikesPublished.HasValue)
            {
                user.LikesPublished = request.LikesPublished.Value;
            }
        }
    }
}

