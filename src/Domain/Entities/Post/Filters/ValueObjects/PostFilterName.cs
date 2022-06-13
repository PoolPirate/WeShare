using Vogen;

namespace WeShare.Domain.Entities;

[ValueObject(typeof(string))]
public readonly partial struct PostFilterName
{
    public static Validation Validate(string? value)
    {
        if (value is null)
        {
            return Validation.Ok;
        }
        if (value.Length > DomainConstraints.ShareFilterNameLengthMaximum)
        {
            return Validation.Invalid($"PostFilterName needs to have at most {DomainConstraints.ShareFilterNameLengthMaximum} characters");
        }
        if (value.Any(x => !CharSet.Url.Contains(x)))
        {
            return Validation.Invalid("Your PostFilterName can only contain letters and numbers'");
        }
        //
        return Validation.Ok;
    }
}
