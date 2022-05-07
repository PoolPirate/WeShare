using Common.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Services;
public interface ISusbcriptionPostPublisher<TSubscription> : IService
    where TSubscription : Subscription
{
    SubscriptionType Type { get; }
    int ChunkSize { get; }
    int DegreeOfParallelism { get; }

    Task<bool> TryPublishPostToSubscriberAsync(Post post, PostContent content, TSubscription subscription, SentPost sentPost,
        CancellationToken cancellationToken);
}