using Vogen;

namespace WeShare.Domain.Common;

[ValueObject(typeof(long))]
public readonly partial struct ByteCount
{
    public static Validation Validate(long value)
    {
        if (value < 0)
        {
            return Validation.Invalid("Byte count can't be negative");
        }
        //
        return Validation.Ok;
    }
}

