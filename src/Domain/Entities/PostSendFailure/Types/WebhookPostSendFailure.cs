using System.Net;

namespace WeShare.Domain.Entities;
public class WebhookPostSendFailure : PostSendFailure
{
    public HttpStatusCode? StatusCode { get; init; }
    public int ResponseLatency { get; init; }

    public static WebhookPostSendFailure Create(PostId postId, SubscriptionId subscriptionId, HttpStatusCode? statusCode, int responseLatency)
    {
        var failure = new WebhookPostSendFailure(postId, subscriptionId, statusCode, responseLatency);
        return failure;
    }

    protected WebhookPostSendFailure(PostId postId, SubscriptionId subscriptionId, HttpStatusCode? statusCode, int responseLatency) 
        : base(postId, subscriptionId)
    {
        StatusCode = statusCode;
        ResponseLatency = responseLatency;
    }
}

