using System.ComponentModel.DataAnnotations;
using WeShare.Domain;

namespace WeShare.WebAPI.Forms;

public class CallbackForm
{
    [MinLength(DomainConstraints.CallbackSecretLength)]
    [MaxLength(DomainConstraints.CallbackSecretLength)]
    [Required]
    public string CallbackSecret { get; set; }
}
