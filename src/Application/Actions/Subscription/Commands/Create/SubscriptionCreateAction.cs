using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WeShare.Application.Common;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;
public class SubscriptionCreateAction
{
    public class Command : IRequest<Result>, IValidatableObject
    {
        [EnumDataType(typeof(SubscriptionType))]
        public SubscriptionType Type { get; }
        public SubscriptionName Name { get; }
        public ShareId ShareId { get; }
        public UserId UserId { get; }

        public Uri? TargetUri { get; }

        public static Command ForDashboard(SubscriptionName name, ShareId shareId, UserId userId)
            => new Command(SubscriptionType.Dashboard, name, shareId, userId, null);
        public static Command ForWebhook(SubscriptionName name, ShareId shareId, UserId userId, Uri? targetUri)
            => new Command(SubscriptionType.Webhook, name, shareId, userId, targetUri);

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (Type)
            {
                case SubscriptionType.Dashboard:
                    break;
                case SubscriptionType.Webhook:
                    if (TargetUri is null)
                    {
                        yield return new ValidationResult("TargetUri is required!");
                    }
                    break;
            }
        }

        private Command(SubscriptionType type, SubscriptionName name, ShareId shareId, UserId userId, Uri? targetUri)
        {
            Type = type;
            Name = name;
            ShareId = shareId;
            UserId = userId;
            TargetUri = targetUri;
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
            var subscription = MakeSubscription(request);

            await Authorizer.EnsureAuthorizationAsync(subscription, SubscriptionCommandOperation.Create, cancellationToken);

            if (!await DbContext.Shares.AnyAsync(x => x.Id == request.ShareId, cancellationToken))
            {
                return new Result(Status.ShareNotFound);
            }

            var transaction = await DbContext.BeginTransactionAsync(cancellationToken);

            DbContext.Subscriptions.Add(subscription);
            await DbContext.Shares
                .Where(x => x.Id == request.ShareId)
                .UpdateFromQueryAsync(x => new Share() { SubscriberCount = x.SubscriberCount + 1 }, cancellationToken: cancellationToken);

            var saveResult = await DbContext.SaveChangesAsync(transaction: transaction, cancellationToken: cancellationToken);

            return saveResult.Status switch
            {
                DbStatus.Success => new Result(Status.Success, subscription),
                _ => throw new UnhandledDbStatusException(saveResult),
            };
        }

        private static Subscription MakeSubscription(Command request)
            => request.Type switch
            {
                SubscriptionType.Dashboard => Subscription.Create(SubscriptionType.Dashboard, request.Name, request.UserId, request.ShareId),
                SubscriptionType.AndroidPushNotification => throw new NotImplementedException(),
                SubscriptionType.MessagerDiscord => throw new NotImplementedException(),
                SubscriptionType.Email => throw new NotImplementedException(),
                SubscriptionType.Webhook => WebhookSubscription.Create(SubscriptionType.Webhook, request.Name, request.UserId, request.ShareId, request.TargetUri!),
                _ => throw new InvalidOperationException(),
            };
    }
}

