using WeShare.Domain.Common;

namespace WeShare.Domain.Entities;
public class Subscription : AuditableEntity
{
    /// <summary>
    /// The unique identifier of the Subscription.
    /// </summary>
    public SubscriptionId Id { get; private set; }

    /// <summary>
    /// The type of the subscription.
    /// </summary>
    public SubscriptionType Type { get; set; }

    /// <summary>
    /// The user who created this subscription.
    /// </summary>
    public User? User { get; init; }

    /// <summary>
    /// The id of the user who created this subscription.
    /// </summary>
    public UserId UserId { get; init; }

    /// <summary>
    /// The share that this subcription applies to.
    /// </summary>
    public Share? Share { get; init; }

    /// <summary>
    /// The id of the share that this subcription applies to.
    /// </summary>
    public ShareId ShareId { get; init; }

    /// <summary>
    /// The last post that was successfully received by the subscription client.
    /// </summary>
    public Post? LastReceivedPost { get; set; } = null!;

    /// <summary>
    /// The id of the last post that was successfully received by the subscription client.
    /// </summary>
    public PostId? LastReceivedPostId { get; set; } = null;

    public static Subscription Create(SubscriptionType type, UserId userId, ShareId shareId, PostId? lastReceivedPostId)
    {
        var subscription = new Subscription(type, userId, shareId, lastReceivedPostId);
        return subscription;
    }

    private Subscription(SubscriptionType type, UserId userId, ShareId shareId, PostId? lastReceivedPostId)
    {
        Type = type;
        UserId = userId;
        ShareId = shareId;
        LastReceivedPostId = lastReceivedPostId;
    }

    public Subscription()
    {
    }
}
