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
    /// The name of the subscription.
    /// </summary>
    public SubscriptionName Name { get; set; }

    /// <summary>
    /// The user who created this subscription.
    /// </summary>
    public User? User { get; init; } //Navigation Property

    /// <summary>
    /// The id of the user who created this subscription.
    /// </summary>
    public UserId UserId { get; init; }

    /// <summary>
    /// The share that this subcription applies to.
    /// </summary>
    public Share? Share { get; init; } //Navigation Property

    /// <summary>
    /// The id of the share that this subcription applies to.
    /// </summary>
    public ShareId ShareId { get; init; }

    /// <summary>
    /// The posts sent to this subscription.
    /// </summary>
    public List<SentPost>? SentPosts { get; init; } //Navigation Property

    public static Subscription Create(SubscriptionType type, SubscriptionName name, UserId userId, ShareId shareId)
    {
        var subscription = new Subscription(type, name, userId, shareId);
        return subscription;
    }

    private Subscription(SubscriptionType type, SubscriptionName name, UserId userId, ShareId shareId)
    {
        Type = type;
        Name = name;
        UserId = userId;
        ShareId = shareId;
    }

    public Subscription()
    {
    }
}
