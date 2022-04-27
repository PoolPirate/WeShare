using Vogen;

namespace WeShare.Domain.Entities;

[ValueObject(typeof(string))]
public readonly partial struct Username
{
    public static Validation Validate(string? value)
    {
        if (value is null)
        {
            return Validation.Invalid("Username is required");
        }
        if (value.Length < DomainConstraints.UsernameLengthMinimum)
        {
            return Validation.Invalid($"Username needs to have at least {DomainConstraints.UsernameLengthMinimum} characters");
        }
        if (value.Length > DomainConstraints.UsernameLengthMaximum)
        {
            return Validation.Invalid($"Username needs to have at most {DomainConstraints.UsernameLengthMaximum} characters");
        }
        if (value.Any(x => !CharSet.LowerLetters.Contains(x) && !CharSet.Numbers.Contains(x) && x != '_'))
        {
            return Validation.Invalid("Your username can only contain lower case letters, numbers and '_'");
        }
        //
        return Validation.Ok;
    }
}

