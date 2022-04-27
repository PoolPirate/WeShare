using System.ComponentModel.DataAnnotations;
using WeShare.Domain;
using WeShare.Extensions;

namespace WeShare.WebAPI.Forms;

public class PasswordResetForm : CallbackForm, IValidatableObject
{
    [MinLength(DomainConstraints.PasswordLengthMinimum)]
    [MaxLength(DomainConstraints.PasswordLengthMaximum)]
    [Required]
    public string Password { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Password.ContainsAny(CharSet.LowerLetters))
        {
            yield return GetMissingCharSetCharResult("lower case letter");
        }
        if (!Password.ContainsAny(CharSet.UpperLetters))
        {
            yield return GetMissingCharSetCharResult("upper case letter");
        }
        if (!Password.ContainsAny(CharSet.Numbers))
        {
            yield return GetMissingCharSetCharResult("number");
        }
        if (!Password.ContainsAny(CharSet.SpecialChars))
        {
            yield return GetMissingCharSetCharResult("special character");
        }
    }

    private ValidationResult GetMissingCharSetCharResult(string charsetName) => new ValidationResult($"Your password must contain at least one {charsetName}!", new[] { nameof(Password) });
}
