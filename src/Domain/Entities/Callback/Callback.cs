using System.Text.Json.Serialization;
using WeShare.Domain.Common;
using WeShare.Domain.Enums;
using WeShare.Domain.Events;

namespace WeShare.Domain.Entities;

public sealed class Callback : AuditableEntity, IHasDomainEvents
{
    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

    /// <summary>
    /// The unique identifier of the callback.
    /// </summary>
    public CallbackId Id { get; private set; }

    /// <summary>
    /// The time at which this callback was successfully sent.
    /// </summary>
    public DateTimeOffset? SuccessfullySentAt { get; set; }

    /// <summary>
    /// The secret of the callback.
    /// </summary>
    public CallbackSecret Secret { get; init; }

    /// <summary>
    /// The action type of the callback.
    /// </summary>
    public CallbackType Type { get; init; }

    /// <summary>
    /// The id of the owner of the callback.
    /// </summary>
    public UserId OwnerId { get; init; }

    /// <summary>
    /// The owner of the callback.
    /// </summary>
    public User? Owner { get; private set; } //Navigation Property

    public static Callback Create(CallbackSecret secret, CallbackType type, UserId ownerId)
    {
        var callback = new Callback(secret, type, ownerId);
        callback.DomainEvents.Add(new CallbackCreatedEvent(callback));
        return callback;
    }

    private Callback(CallbackSecret secret, CallbackType type, UserId ownerId)
    {
        Secret = secret;
        Type = type;
        OwnerId = ownerId;
    }

    [JsonConstructor]
#pragma warning disable CS8618 
    public Callback()
#pragma warning restore CS8618
    {
    }
}
