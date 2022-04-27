using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace WeShare.Infrastructure.Persistence;
public class StronglyTypedIdValueGenerator<T> : ValueGenerator<T> where T : struct
{
    public override bool GeneratesTemporaryValues
        => true;

    public override T Next(EntityEntry entry) 
        => new T();
}

