using WeShare.Domain.Common;

namespace WeShare.Domain.Entities;
public class PostSendFailure : AuditableEntity
{
    /// <summary>
    /// The unique identifier of this PostSendFailure.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The id of the post of this failure.
    /// </summary>
    public PostId PostId { get; init; }

    /// <summary>
    /// The id of the subscription of this failure.
    /// </summary>
    public SubscriptionId SubscriptionId { get; init; }

    /// <summary>
    /// The type of the subscription of this failure.
    /// </summary>
    public SubscriptionType SubscriptionType { get; init; }

    /// <summary>
    /// The sent post that this error belongs to.
    /// </summary>
    public SentPost? SentPost { get; init; } //Naviagtion Property

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

