namespace WeShare.Domain.Entities;
public class WebhookSubscription : Subscription
{
    public Uri TargetUrl { get; set; }

    public static WebhookSubscription Create(SubscriptionType type, SubscriptionName name, UserId userId, ShareId shareId, Uri targetUrl)
    {
        var subscription = new WebhookSubscription(type, name, userId, shareId, targetUrl);
        return subscription;
    }

    protected WebhookSubscription(SubscriptionType type, SubscriptionName name, UserId userId, ShareId shareId, Uri targetUrl) 
        : base(type, name, userId, shareId)
    {
        TargetUrl = targetUrl;
    }
}

