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
    /// Whether or not this is a permanent error and no more retries will be done or not.
    /// </summary>
    public bool IsFinal { get; init; }

    /// <summary>
    /// The type of the subscription of this failure.
    /// </summary>
    public PostSendFailureType Type { get; init; }

    /// <summary>
    /// The sent post that this error belongs to.
    /// </summary>
    public SentPost? SentPost { get; init; } //Naviagtion Property

    public static PostSendFailure CreateInternalError(PostId postId, SubscriptionId subscriptionId)
        => new PostSendFailure(postId, subscriptionId, false, PostSendFailureType.InternalError);

    protected PostSendFailure(PostId postId, SubscriptionId subscriptionId, bool isFinal, PostSendFailureType type)
    {
        Id = Guid.NewGuid();
        PostId = postId;
        SubscriptionId = subscriptionId;
        IsFinal = isFinal;
        Type = type;
    }
    public PostSendFailure()
    {
    }
}

