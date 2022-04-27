namespace WeShare.Domain.Common;

public abstract class AuditableEntity
{
    /// <summary>
    /// The time at which this entity was added to the database.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}
