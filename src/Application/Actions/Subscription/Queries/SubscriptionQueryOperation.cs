namespace WeShare.Application.Actions.Queries;
public enum SubscriptionQueryOperation : byte
{
    ReadInfo,
    ReadUnreceivedPosts,
    ReadReceivedPosts,
    ReadPendingPosts,
}