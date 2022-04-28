export class SubscriptionInfo {
  id: number;
  createdAt: Date;
  type: SubscriptionType;
  name: string;
}

export class SubscriptionSnippet {
  id: number;
  name: string;
}

enum SubscriptionType {
  Dashboard = 0,

  AndroidPushNotification = 100,

  MessagerDiscord = 200,

  Email = 300,

  Webhook = 400,
}
