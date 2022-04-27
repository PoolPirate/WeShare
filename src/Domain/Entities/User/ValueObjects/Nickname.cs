using Vogen;

namespace WeShare.Domain.Entities;

[ValueObject(typeof(string))]
public readonly partial struct Nickname
{
    public static Validation Validate(string? value)
    {
        if (value is null)
        {
            return Validation.Ok;
        }

        if (value.Length < DomainConstraints.NicknameLengthMinimum)
        {
            return Validation.Invalid($"Nickname needs to have at least {DomainConstraints.NicknameLengthMinimum} characters");
        }
        if (value.Length > DomainConstraints.NicknameLengthMaximum)
        {
            return Validation.Invalid($"Nickname needs to have at most {DomainConstraints.NicknameLengthMaximum} characters");
        }
        //
        return Validation.Ok;
    }
}

