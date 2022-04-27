using Vogen;
using WeShare.Extensions;

namespace WeShare.Domain.Entities;

[ValueObject(typeof(string))]
public readonly partial struct PlainTextPassword
{
    public static Validation Validate(string? value)
    {
        if (value is null)
        {
            return Validation.Invalid("Password is required");
        }
        if (value.Length < DomainConstraints.PasswordLengthMinimum)
        {
            return Validation.Invalid($"Password needs to have at least {DomainConstraints.NicknameLengthMinimum} characters");
        }
        if (value.Length > DomainConstraints.PasswordLengthMaximum)
        {
            return Validation.Invalid($"Password needs to have at most {DomainConstraints.NicknameLengthMaximum} characters");
        }
        if (!value.ContainsAny(CharSet.LowerLetters))
        {
            return Validation.Invalid("Password needs at least one lower case letter");
        }
        if (!value.ContainsAny(CharSet.UpperLetters))
        {
            return Validation.Invalid("Password needs at least one upper case letter");
        }
        if (!value.ContainsAny(CharSet.Numbers))
        {
            return Validation.Invalid("Password needs at least one number");
        }
        if (!value.ContainsAny(CharSet.SpecialChars))
        {
            return Validation.Invalid("Password needs at least one special character");
        }
        //
        return Validation.Ok;
    }
}

