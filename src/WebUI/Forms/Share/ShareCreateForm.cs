using System.ComponentModel.DataAnnotations;
using WeShare.Domain;

namespace WeShare.WebAPI.Forms;

public class ShareCreateForm
{
    /// <summary>
    /// The name of the share to create.
    /// </summary>
    [MinLength(DomainConstraints.ShareNameLengthMinimum)]
    [MaxLength(DomainConstraints.ShareNameLengthMaximum)]
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Whether or not the share is private.
    /// </summary>
    public bool IsPrivate { get; set; }

    /// <summary>
    /// The description of the share to create.
    /// </summary>
    [MaxLength(DomainConstraints.ShareDescriptionLengthMaximum)]
    public string Description { get; set; }

    /// <summary>
    /// The markdown readme file of the share.
    /// </summary>
    [MaxLength(DomainConstraints.ShareReadmeLengthMaximum)]
    public string Readme { get; set; }

    public ShareCreateForm()
    {
    }
}
