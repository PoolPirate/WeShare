﻿namespace WeShare.Domain.Entities;
public class SentPost
{
    /// <summary>
    /// The id of the post that has been sent.
    /// </summary>
    public PostId PostId { get; private set; }

    /// <summary>
    /// The post that has been sent.
    /// </summary>
    public Post? Post { get; private set; }

    /// <summary>
    /// The id of the subscription that this has been sent to.
    /// </summary>
    public SubscriptionId SubscriptionId { get; private set; }

    /// <summary>
    /// The subscription that this has been sent to.
    /// </summary>
    public Subscription? Subscription { get; private set; }

    /// <summary>
    /// Whether or not the post was successfully received.
    /// </summary>
    public bool Received { get; private set; } = false;
    /// <summary>
    /// The time at which the post was successfully received.
    /// </summary>
    public DateTimeOffset? ReceivedAt { get; private set; } = null;

    /// <summary>
    /// Whether or not there will be further attempts to publish this post.
    /// </summary>
    public bool IsFinal { get; private set; } = false;

    /// <summary>
    /// The amount of attempts that have been executed to get to current state.
    /// </summary>
    public short Attempts { get; private set; } = 0;

    /// <summary>
    /// The failure reports that have been created during failed attempts.
    /// </summary>
    public List<PostSendFailure>? PostSendFailures { get; init; } = null; //Navigation Property

    public void SetReceived()
    {
        Received = true;
        ReceivedAt = DateTimeOffset.UtcNow;
        IsFinal = true;
    }
    public void SetFailed()
        => IsFinal = true;

    public void IncrementAttempts()
        => Attempts++;

    public static SentPost Create(PostId postId, SubscriptionId subscriptionId)
        => new SentPost(postId, subscriptionId);

    private SentPost(PostId postId, SubscriptionId subscriptionId)
    {
        PostId = postId;
        SubscriptionId = subscriptionId;
    }
    public SentPost()
    {
    }
}

