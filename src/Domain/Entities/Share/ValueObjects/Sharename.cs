using Vogen;

namespace WeShare.Domain.Entities;

[ValueObject(typeof(string))]
public readonly partial struct ShareName
{
    public static Validation Validate(string? value)
    {
        if (value is null)
        {
            return Validation.Invalid("ShareName is required");
        }
        if (value.Length < DomainConstraints.ShareNameLengthMinimum)
        {
            return Validation.Invalid($"ShareName needs to have at least {DomainConstraints.ShareNameLengthMinimum} characters");
        }
        if (value.Length > DomainConstraints.ShareNameLengthMaximum)
        {
            return Validation.Invalid($"ShareName needs to have at most {DomainConstraints.ShareNameLengthMaximum} characters");
        }
        if (value.Any(x => !CharSet.Url.Contains(x)))
        {
            return Validation.Invalid("Your ShareName can only contain letters and numbers'");
        }
        //
        return Validation.Ok;
    }
}

