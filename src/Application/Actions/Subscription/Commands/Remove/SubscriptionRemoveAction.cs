using MediatR;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class SubscriptionRemoveAction
{
    public class Command : IRequest<Result>
    {
        public SubscriptionId SubscriptionId { get; set; }

        public Command(SubscriptionId subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }
    }

    public enum Status : byte
    {
        Success,
        SubscriptionNotFound,
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
            var subscription = await DbContext.Subscriptions
                .Where(x => x.Id == request.SubscriptionId)
                .SingleOrDefaultAsync(cancellationToken);

            if (subscription is null)
            {
                return new Result(Status.SubscriptionNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(subscription, SubscriptionCommandOperation.Remove, cancellationToken);

            DbContext.Subscriptions.Remove(subscription);

            var saveResult = await DbContext.SaveChangesAsync(DbStatus.ConcurrencyEntryDeleted, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success),
                DbStatus.ConcurrencyEntryDeleted => new Result(Status.SubscriptionNotFound),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

