using WeShare.Domain.Common;
using WeShare.Domain.Entities;

namespace WeShare.Domain.Events;
public class CallbackCreatedEvent : DomainEvent
{
    public Callback Callback { get; }

    public CallbackCreatedEvent(Callback callback)
    {
        Callback = callback;
    }
}

