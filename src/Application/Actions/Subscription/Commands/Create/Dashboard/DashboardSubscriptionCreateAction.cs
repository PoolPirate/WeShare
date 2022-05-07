using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class DashboardSubscriptionCreateAction
{
    public class Command : IRequest<Result>
    {
        public SubscriptionName Name { get; }
        public ShareId ShareId { get; }
        public UserId UserId { get; }

        public Command(SubscriptionName name, ShareId shareId, UserId userId)
        {
            Name = name;
            ShareId = shareId;
            UserId = userId;
        }
    }

    public enum Status : byte
    {
        Success,
        ShareNotFound,
    }

    public record Result(Status Status, Subscription? Subscription = null);

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
            var subscription = Subscription.Create(SubscriptionType.Dashboard, request.Name, request.UserId, request.ShareId);

            await Authorizer.EnsureAuthorizationAsync(subscription, SubscriptionCommandOperation.Create, cancellationToken);

            if (!await DbContext.Shares.AnyAsync(x => x.Id == request.ShareId, cancellationToken))
            {
                return new Result(Status.ShareNotFound);
            }

            var transaction = await DbContext.BeginTransactionAsync(cancellationToken);

            DbContext.Subscriptions.Add(subscription);
            int updateCount = await DbContext.Shares
                .Where(x => x.Id == request.ShareId)
                .Where(x => !x.IsPrivate || x.OwnerId == request.UserId)
                .UpdateFromQueryAsync(x => new Share() { SubscriberCount = x.SubscriberCount + 1 }, cancellationToken: cancellationToken);

            if (updateCount == 0) //Share is private and liker is not owner
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new ForbiddenAccessException();
            }

            var saveResult = await DbContext.SaveChangesAsync(transaction: transaction, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success, subscription),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }
    }
}

