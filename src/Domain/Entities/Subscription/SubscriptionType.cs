namespace WeShare.Domain.Entities;
public enum SubscriptionType
{
    Dashboard = 0,

    AndroidPushNotification = 100,

    MessagerDiscord = 200,

    Email = 300,

    Webhook = 400,
}

public static class SubscriptionTypeExtensions
{
    public static bool SupportsMarkAsReceivedAction(this SubscriptionType subscriptionType)
        => subscriptionType switch
        {
            SubscriptionType.Dashboard => true,

            SubscriptionType.AndroidPushNotification => false,
            SubscriptionType.MessagerDiscord => false,
            SubscriptionType.Email => false,
            SubscriptionType.Webhook => false,
            _ => throw new InvalidOperationException(),
        };
}

