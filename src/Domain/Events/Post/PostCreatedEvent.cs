using WeShare.Domain.Common;
using WeShare.Domain.Entities;

namespace WeShare.Domain.Events;
public class PostCreatedEvent : DomainEvent
{
    public Post Post { get; }

    public PostCreatedEvent(Post post)
    {
        Post = post;
    }
}

