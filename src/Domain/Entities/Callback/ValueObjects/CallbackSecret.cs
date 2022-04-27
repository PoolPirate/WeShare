using Vogen;

namespace WeShare.Domain.Entities;
[ValueObject(typeof(string))]
public readonly partial struct CallbackSecret
{
    public static Validation Validate(string? value)
    {
        if (value is null)
        {
            return Validation.Invalid("CallbackSecret is required");
        }
        if (value.Length != DomainConstraints.CallbackSecretLength)
        {
            return Validation.Invalid($"CallbackSecret must be exactly {DomainConstraints.CallbackSecretLength} characters long");
        }

        //
        return Validation.Ok;
    }
}

