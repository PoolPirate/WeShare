using System.ComponentModel.DataAnnotations;
using WeShare.Domain;
using WeShare.Extensions;

namespace WeShare.WebAPI.Forms;

public class UserCreateForm : IValidatableObject
{
    [MinLength(DomainConstraints.UsernameLengthMinimum)]
    [MaxLength(DomainConstraints.UsernameLengthMaximum)]
    [Required]
    public string Username { get; set; }

    [EmailAddress]
    [MaxLength(DomainConstraints.EmailLengthMaximum)]
    [Required]
    public string Email { get; set; }

    [MinLength(DomainConstraints.PasswordLengthMinimum)]
    [MaxLength(DomainConstraints.PasswordLengthMaximum)]
    [Required]
    public string Password { get; set; }

    public UserCreateForm()
    {
    }

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
        if (Username.Any(x => !CharSet.LowerLetters.Contains(x) && !CharSet.UpperLetters.Contains(x) && !CharSet.Numbers.Contains(x) && x != '_'))
        {
            yield return new ValidationResult("Your username can only contain letters, numbers and '_'");
        }
    }

    private ValidationResult GetMissingCharSetCharResult(string charsetName) => new ValidationResult($"Your password must contain at least one {charsetName}!", new[] { nameof(Password) });
}
