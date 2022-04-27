using Vogen;

namespace WeShare.Domain.Entities;
[ValueObject(typeof(string))]
public readonly partial struct ShareSecret
{
    public static Validation Validate(string? value)
    {
        if (value is null)
        {
            return Validation.Invalid("ShareSecret is required");
        }
        if (value.Length != DomainConstraints.ShareSecretLength)
        {
            return Validation.Invalid($"ShareSecret must be exactly {DomainConstraints.ShareSecretLength} characters long");
        }

        //
        return Validation.Ok;
    }
}

