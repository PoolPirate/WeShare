using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;

public class ShareInfoDto : IMapFrom<Share>
{
    public long Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public int LikeCount { get; private set; }
    public int SubscriberCount { get; private set; }

    public string Name { get; private set; } = null!;
    public bool IsPrivate { get; private set; }
    public string Description { get; private set; } = null!;
    public string Readme { get; private set; } = null!;

    public long OwnerId { get; private set; }
}
