using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class DiscordSubscriptionCreateAction
{
    public class Command : IRequest<Result>
    {
        public SubscriptionName Name { get; }
        public ShareId ShareId { get; }
        public UserId UserId { get; }
        public ServiceConnectionId ServiceConnectionId { get; }

        public Command(SubscriptionName name, ShareId shareId, UserId userId, ServiceConnectionId serviceConnectionId)
        {
            Name = name;
            ShareId = shareId;
            UserId = userId;
            ServiceConnectionId = serviceConnectionId;
        }
    }

    public enum Status : byte
    {
        Success,
        ShareNotFound,
        ServiceConnectionNotFound,
        DiscordUnavailable,
    }

    public record Result(Status Status, Subscription? Subscription = null);

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IShareContext DbContext;
        private readonly IAuthorizer Authorizer;
        private readonly IDiscordClient DiscordClient;

        public Handler(IShareContext dbContext, IAuthorizer authorizer, IDiscordClient discordClient)
        {
            DbContext = dbContext;
            Authorizer = authorizer;
            DiscordClient = discordClient;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await DbContext.Shares.AnyAsync(x => x.Id == request.ShareId, cancellationToken))
            {
                return new Result(Status.ShareNotFound);
            }

            var serviceConnection = await DbContext.DiscordConnections
                .Where(x => x.Id == request.ServiceConnectionId)
                .SingleOrDefaultAsync(cancellationToken);

            if (serviceConnection is null)
            {
                return new Result(Status.ServiceConnectionNotFound);
            }

            await Authorizer.EnsureAuthorizationAsync(serviceConnection, ServiceConnectionCommandOperation.UseForSubscription, cancellationToken);

            var channelIdResponse = await DiscordClient.GetDMChannelId(serviceConnection.DiscordId, cancellationToken);

            switch (channelIdResponse.Status)
            {
                case DiscordStatus.Success:
                    break;
                case DiscordStatus.RateLimited:
                case DiscordStatus.Unavailable:
                    return new Result(Status.DiscordUnavailable);
                case DiscordStatus.Forbidden:
                default: 
                    throw new InvalidOperationException();
            }

            var subscription = DiscordSubscription.Create(request.Name, request.UserId, request.ShareId, channelIdResponse.Value);

            await Authorizer.EnsureAuthorizationAsync(subscription, SubscriptionCommandOperation.Create, cancellationToken);

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

