namespace WeShare.Domain.Entities;
public enum SubscriptionType
{
    Dashboard,
    Webhook
}

public static class SubscriptionTypeExtensions
{
    public static bool SupportsMarkAsReceivedAction(this SubscriptionType subscriptionType)
        => subscriptionType switch
        {
            SubscriptionType.Dashboard => true,
            SubscriptionType.Webhook => false,
            _ => throw new InvalidOperationException(),
        };
}

