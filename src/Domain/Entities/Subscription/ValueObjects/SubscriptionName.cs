using Vogen;

namespace WeShare.Domain.Entities;

[ValueObject(typeof(string))]
public readonly partial struct SubscriptionName
{
    public static Validation Validate(string? value)
    {
        if (value is null)
        {
            return Validation.Invalid($"SubscriptionName cant be empty");
        }

        if (value.Length < DomainConstraints.SubscriptionnameLengthMinimum)
        {
            return Validation.Invalid($"SubscriptionName needs to have at least {DomainConstraints.SubscriptionnameLengthMinimum} characters");
        }
        if (value.Length > DomainConstraints.SubscriptionnameLengthMaximum)
        {
            return Validation.Invalid($"SubscriptionNameSubscriptionName needs to have at most {DomainConstraints.SubscriptionnameLengthMaximum} characters");
        }
        //
        return Validation.Ok;
    }
}

