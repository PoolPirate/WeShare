using WeShare.Domain.Common;

namespace WeShare.Domain.Entities;
public class ServiceConnection : AuditableEntity
{
    public ServiceConnectionId Id { get; init; }
    public UserId UserId { get; init; }

    public ServiceConnectionType Type { get; init; }

    protected ServiceConnection(UserId userId, ServiceConnectionType type)
    {
        UserId = userId;
        Type = type;
    }
}

