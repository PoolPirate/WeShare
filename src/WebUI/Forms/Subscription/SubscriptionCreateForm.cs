namespace WeShare.WebAPI.Forms;

public class SubscriptionCreateForm
{
    /// <summary>
    /// The id of the share to subscribe to.
    /// </summary>
    public long ShareId { get; }

    /// <summary>
    /// The id of the user who is subscribing.
    /// </summary>
    public long UserId { get; }

    /// <summary>
    /// The name of the subscription.
    /// </summary>
    public string Name { get; }
}
