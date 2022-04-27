using System.Text.Json.Serialization;
using WeShare.Domain.Common;

namespace WeShare.Domain.Entities;

public class Like : AuditableEntity
{
    /// <summary>
    /// The id of the owner of the like.
    /// </summary>
    public UserId OwnerId { get; init; }

    /// <summary>
    /// The id of the share that this applies to.
    /// </summary>
    public ShareId ShareId { get; init; }

    /// <summary>
    /// The user who owns this like.
    /// </summary>
    public User? Owner { get; init; } //Navigation Property

    /// <summary>
    /// The share that this like applies to.
    /// </summary>
    public Share? Share { get; init; } //Navigation Property

    public static Like Create(UserId ownerId, ShareId shareId)
        => new Like(ownerId, shareId);

    private Like(UserId ownerId, ShareId shareId)
    {
        OwnerId = ownerId;
        ShareId = shareId;
    }

    [JsonConstructor]
    public Like()
    {
    }
}
