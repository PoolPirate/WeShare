using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WeShare.Domain.Common;
using WeShare.Domain.Enums;

namespace WeShare.Domain.Entities;
public sealed class Share : AuditableEntity
{
    /// <summary>
    /// The id of the share.
    /// </summary>
    public ShareId Id { get; init; }

    /// <summary>
    /// The amount of likes that this share received.
    /// </summary>
    public int LikeCount { get; set; }

    /// <summary>
    /// The amounts of subscribers that this share has.
    /// </summary>
    public int SubscriberCount { get; set; }

    /// <summary>
    /// The secret of the share used for posting.
    /// </summary>
    public ShareSecret Secret { get; set; }

    /// <summary>
    /// The name of the share.
    /// </summary>
    public Sharename Name { get; set; }

    /// <summary>
    /// The description of the share.
    /// </summary>
    [MaxLength(DomainConstraints.ShareDescriptionLengthMaximum)]
    public string Description { get; set; }

    /// <summary>
    /// The markdown readme file of this share.
    /// </summary>
    [MaxLength(DomainConstraints.ShareReadmeLengthMaximum)]
    public string Readme { get; set; }

    /// <summary>
    /// The type of processing that is applied to the headers of posts before distribution.
    /// </summary>
    public HeaderProcessingType HeaderProcessingType { get; set; }

    /// <summary>
    /// The type of processing that is applied to the body of posts before distribution.
    /// </summary>
    public PayloadProcessingType PayloadProcessingType { get; set; }

    /// <summary>
    /// The id of the owner of this share.
    /// </summary>
    public UserId OwnerId { get; init; }

    /// <summary>
    /// The user who owns this share.
    /// </summary>
    public User? Owner { get; init; } //Navigation Property

    /// <summary>
    /// The likes that apply to this share.
    /// </summary>
    public List<Like>? Likes { get; init; } //Navigation Property

    /// <summary>
    /// The posts submitted to this share.
    /// </summary>
    public List<Post>? Posts { get; init; } //Navigation Property

    public static Share Create(UserId ownerId, Sharename name, string description, string readme, ShareSecret secret)
        => new Share(ownerId, name, description, readme, secret);

    private Share(UserId ownerId, Sharename name, string description, string readme, ShareSecret secret)
    {
        OwnerId = ownerId;
        Name = name;
        Description = description;
        Readme = readme;
        Secret = secret;
        HeaderProcessingType = HeaderProcessingType.NoProcessing;
        PayloadProcessingType = PayloadProcessingType.NoProcessing;
    }

    [JsonConstructor]
#pragma warning disable CS8618
    public Share()
#pragma warning restore CS8618
    {
    }
}
