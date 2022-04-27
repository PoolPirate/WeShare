using WeShare.Domain.Common;
using WeShare.Domain.Events;

namespace WeShare.Domain.Entities;
public class Post : AuditableEntity, IHasDomainEvents
{
    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

    /// <summary>
    /// The Id of the post.
    /// </summary>
    public PostId Id { get; private set; }

    /// <summary>
    /// The amount of bytes that this posts headers contains.
    /// </summary>
    public ByteCount HeadersSize { get; init; }

    /// <summary>
    /// The amount of bytes that this posts payload contains.
    /// </summary>
    public ByteCount PayloadSize { get; init; }

    /// <summary>
    /// The id of the share that this has been posted in.
    /// </summary>
    public ShareId ShareId { get; init; }

    /// <summary>
    /// The share that this has been posted in.
    /// </summary>
    public Share? Share { get; init; } //Navigation Property

    public static Post Create(ByteCount headersSize, ByteCount payloadSize, ShareId shareId)
    {
        var post = new Post(headersSize, payloadSize, shareId);
        post.DomainEvents.Add(new PostCreatedEvent(post));
        return post;
    }

    private Post(ByteCount headersSize, ByteCount payloadSize, ShareId shareId)
    {
        HeadersSize = headersSize;
        PayloadSize = payloadSize;
        ShareId = shareId;
    }
}

