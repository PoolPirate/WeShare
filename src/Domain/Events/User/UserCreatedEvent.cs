using WeShare.Domain.Common;
using WeShare.Domain.Entities;

namespace WeShare.Domain.Events;
public class UserCreatedEvent : DomainEvent
{
    public User User { get; }

    public UserCreatedEvent(User user)
    {
        User = user;
    }
}
