using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WeShare.Domain;
using WeShare.WebAPI.Converters;

namespace WeShare.WebAPI.Forms;

public class ProfileUpdateForm
{
    [MinLength(DomainConstraints.NicknameLengthMinimum)]
    [MaxLength(DomainConstraints.NicknameLengthMaximum)]
    public string? Nickname { get; set; }

    [JsonConverter(typeof(NullableBooleanConverter))]
    public bool? LikesPublished { get; set; }
}
