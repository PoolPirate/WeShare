using WeShare.Domain.Common;
using WeShare.Domain.Entities;

namespace WeShare.Domain.Events;
public class UserEmailUpdatedEvent : DomainEvent
{
    public UserId UserId { get; }
    public string OldEmail { get; }
    public string NewEmail { get; }

    public UserEmailUpdatedEvent(UserId userId, string oldEmail, string newEmail)
    {
        UserId = userId;
        OldEmail = oldEmail;
        NewEmail = newEmail;
    }
}

