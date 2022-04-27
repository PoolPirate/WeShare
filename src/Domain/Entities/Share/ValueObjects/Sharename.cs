using Vogen;

namespace WeShare.Domain.Entities;

[ValueObject(typeof(string))]
public readonly partial struct Sharename
{
    public static Validation Validate(string? value)
    {
        if (value is null)
        {
            return Validation.Invalid("Sharename is required");
        }
        if (value.Length < DomainConstraints.ShareNameLengthMinimum)
        {
            return Validation.Invalid($"Sharename needs to have at least {DomainConstraints.ShareNameLengthMinimum} characters");
        }
        if (value.Length > DomainConstraints.ShareNameLengthMaximum)
        {
            return Validation.Invalid($"Sharename needs to have at most {DomainConstraints.ShareNameLengthMaximum} characters");
        }
        if (value.Any(x => !CharSet.Url.Contains(x)))
        {
            return Validation.Invalid("Your share name can only contain letters and numbers'");
        }
        //
        return Validation.Ok;
    }
}

