using System.ComponentModel.DataAnnotations;
using WeShare.Domain;

namespace WeShare.WebAPI.Forms;

public class AccountUpdateForm : IValidatableObject
{
    [MinLength(DomainConstraints.UsernameLengthMinimum)]
    [MaxLength(DomainConstraints.UsernameLengthMaximum)]
    public string? Username { get; set; }

    [MaxLength(DomainConstraints.EmailLengthMaximum)]
    [EmailAddress]
    public string? Email { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Username is not null &&
            Username.Any(x => !CharSet.LowerLetters.Contains(x) && !CharSet.UpperLetters.Contains(x) && !CharSet.Numbers.Contains(x) && x != '_'))
        {
            yield return new ValidationResult("Your username can only contain letters, numbers and '_'");
        }
    }
}
