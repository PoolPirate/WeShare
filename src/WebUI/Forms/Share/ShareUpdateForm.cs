using System.ComponentModel.DataAnnotations;
using WeShare.Domain;

namespace WeShare.WebAPI.Forms;

public class ShareUpdateForm
{
    /// <summary>
    /// The name of the share to create.
    /// </summary>
    [MinLength(DomainConstraints.ShareNameLengthMinimum)]
    [MaxLength(DomainConstraints.ShareNameLengthMaximum)]
    public string? Name { get; set; }

    /// <summary>
    /// The updated description of the share or null.
    /// </summary>
    [MaxLength(DomainConstraints.ShareDescriptionLengthMaximum)]
    public string? Description { get; set; }

    /// <summary>
    /// The updated markdown readme file of the share or null.
    /// </summary>
    [MaxLength(DomainConstraints.ShareReadmeLengthMaximum)]
    public string? Readme { get; set; }
}
