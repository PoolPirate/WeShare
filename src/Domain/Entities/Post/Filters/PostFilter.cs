using WeShare.Domain.Common;

namespace WeShare.Domain.Entities;

public abstract class PostFilter : AuditableEntity
{
    /// <summary>
    /// The unique identifier of the PostFilter.
    /// </summary>
    public PostFilterId Id { get; init; }

    /// <summary>
    /// The type of the PostFilter.
    /// </summary>
    public PostFilterType Type { get; init; }

    /// <summary>
    /// The name of the PostFilter.
    /// </summary>
    public PostFilterName Name { get; set; }

    /// <summary>
    /// The unique identifier of the share that this PostFilter belongs to.
    /// </summary>
    public ShareId ShareId { get; init; }

    /// <summary>
    /// The share that this PostFilter belongs to.
    /// </summary>
    public Share? Share { get; init; }

    protected PostFilter(ShareId shareId, PostFilterType type, PostFilterName name)
    {
        ShareId = shareId;
        Type = type;
        Name = name;
    }
}
