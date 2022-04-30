using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Forms;

public class SubscriptionCreateForm
{
    /// <summary>
    /// The id of the share to subscribe to.
    /// </summary>
    public long ShareId { get; set; }

    /// <summary>
    /// The id of the user who is subscribing.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// The type of the subscription.
    /// </summary>
    public SubscriptionType Type { get; set; }

    /// <summary>
    /// The name of the subscription.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The target url of the webhook.
    /// </summary>
    /// <remarks>
    /// Only for <see cref="SubscriptionType.Webhook"/>
    /// </remarks>
    public Uri? TargetUrl { get; set; }
}
