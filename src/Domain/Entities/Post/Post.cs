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
    public ByteCount HeadersSize { get; private set; } = ByteCount.From(0);

    /// <summary>
    /// The amount of bytes that this posts payload contains.
    /// </summary>
    public ByteCount PayloadSize { get; private set; } = ByteCount.From(0);

    /// <summary>
    /// The id of the share that this has been posted in.
    /// </summary>
    public ShareId ShareId { get; init; }

    /// <summary>
    /// The share that this has been posted in.
    /// </summary>
    public Share? Share { get; init; } //Navigation Property

    /// <summary>
    /// The history of who this post was sent to.
    /// </summary>
    public List<SentPost>? SentPosts { get; init; }

    public void SetMetadata(ByteCount headerSize, ByteCount payloadSize)
    {
        HeadersSize = headerSize;
        PayloadSize = payloadSize;
    }

    public static Post Create(ShareId shareId)
    {
        var post = new Post(shareId);
        post.DomainEvents.Add(new PostCreatedEvent(post));
        return post;
    }

    private Post(ShareId shareId)
    {
        ShareId = shareId;
    }
    public Post()
    {
    }
}

