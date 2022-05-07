using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class ServiceConnectionRemoveAction
{
    public class Command : IRequest<Result>
    {
        public UserId UserId { get; }
        public ServiceConnectionId ServiceConnectionId { get; }

        public Command(UserId userId, ServiceConnectionId serviceConnectionId)
        {
            UserId = userId;
            ServiceConnectionId = serviceConnectionId;
        }
    }

    public enum Status : byte
    {
        Success,
        UserNotFound,
        ServiceConnectionNotFound,
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
            if (!await DbContext.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken))
            {
                return new Result(Status.UserNotFound);
            }

            var serviceConnection = await DbContext.ServiceConnections
                .Where(x => x.Id == request.ServiceConnectionId)
                .SingleOrDefaultAsync(cancellationToken);

            if (serviceConnection is null)
            {
                return new Result(Status.ServiceConnectionNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(serviceConnection, ServiceConnectionCommandOperation.Remove, cancellationToken);

            DbContext.ServiceConnections.Remove(serviceConnection);

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.ConcurrencyEntryDeleted, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.ConcurrencyEntryDeleted => new Result(Status.ServiceConnectionNotFound),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

