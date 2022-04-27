using System.ComponentModel.DataAnnotations;
using WeShare.Domain;

namespace WeShare.WebAPI.Forms;

public class AccountRequestPassswordResetForm
{
    [EmailAddress]
    [MaxLength(DomainConstraints.EmailLengthMaximum)]
    [Required]
    public string Email { get; set; }
}
