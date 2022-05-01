using WeShare.Domain.Common;

namespace WeShare.Domain.Entities;
public class PostSendFailure : AuditableEntity
{
    public Guid Id { get; init; }
    public PostId PostId { get; init; }
    public SubscriptionId SubscriptionId { get; init; }
    public SubscriptionType SubscriptionType { get; init; }

    protected PostSendFailure(PostId postId, SubscriptionId subscriptionId)
    {
        Id = Guid.NewGuid();
        PostId = postId;
        SubscriptionId = subscriptionId;
    }
    public PostSendFailure()
    {
    }
}

