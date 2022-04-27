using System.Collections;
using System.Reflection;
using WeShare.Domain.Common;

namespace WeShare.Infrastructure.Persistence.Concurrency;

public sealed class PropertyMerger
{
    private readonly Dictionary<PropertyInfo, MergeStrategy> PropertyMergeStrategyCache;
    private readonly ReaderWriterLockSlim PropertyMergeStrategyCacheLock;

    public PropertyMerger()
    {
        PropertyMergeStrategyCache = new Dictionary<PropertyInfo, MergeStrategy>();
        PropertyMergeStrategyCacheLock = new ReaderWriterLockSlim();
    }

    public object Merge(PropertyInfo property, object original, object current, object other)
    {
        var strategy = GetOrLoadMergeStrategyForPropertyInfo(property);

        switch (strategy)
        {
            case MergeStrategy.TakeLargest:
                return Comparer.Default.Compare(current, other) < 0
                    ? other
                    : current;
            case MergeStrategy.TakeSmallest:
                return Comparer.Default.Compare(current, other) > 0
                    ? other
                    : current;
            case MergeStrategy.AddChanges:
                if (current.GetType() != original.GetType() || other.GetType() != original.GetType())
                {
                    throw new Exception($"Property {property.Name} does not support MergeStrategy.AddChanges");
                }

                if (current.GetType() == typeof(ulong))
                {
                    ulong originalNumber = (ulong)original;
                    return (ulong)current + (ulong)other - originalNumber;
                }
                else if (current.GetType() == typeof(long))
                {
                    long originalNumber = (long)original;
                    return (long)current + (long)other - originalNumber;
                }
                else if (current.GetType() == typeof(uint))
                {
                    uint originalNumber = (uint)original;
                    return (uint)current + (uint)other - originalNumber;
                }
                else if (current.GetType() == typeof(int))
                {
                    int originalNumber = (int)original;
                    return (int)current + (int)other - originalNumber;
                }
                else if (current.GetType() == typeof(ushort))
                {
                    ushort originalNumber = (ushort)original;
                    return (ushort)current + (ushort)other - originalNumber;
                }
                else if (current.GetType() == typeof(short))
                {
                    short originalNumber = (short)original;
                    return (short)current + (short)other - originalNumber;
                }
                else if (current.GetType() == typeof(byte))
                {
                    byte originalNumber = (byte)original;
                    return (byte)current + (byte)other - originalNumber;
                }

                try
                {
                    return (dynamic)current - (dynamic)original + (dynamic)other - (dynamic)original;
                }
                catch
                {
                    throw new Exception($"Property {property.Name} does not support MergeStrategy.AddChanges");
                }
            case MergeStrategy.TakeCurrent:
                return current;
            case MergeStrategy.TakeOther:
                return other;
            case MergeStrategy.TakeOriginal:
                return original;
            default:
                throw new InvalidOperationException();
        }
    }

    private MergeStrategy GetOrLoadMergeStrategyForPropertyInfo(PropertyInfo property)
    {
        PropertyMergeStrategyCacheLock.EnterUpgradeableReadLock();

        try
        {
            if (!PropertyMergeStrategyCache.TryGetValue(property, out var strategy))
            {
                var mergeStrategyAttribute = property.GetCustomAttribute<MergeStrategyAttribute>();

                if (mergeStrategyAttribute == null)
                {
                    throw new InvalidOperationException($"Could not find a MergeStrategyAttribute for the property {property.Name}");
                }
                Console.Write("Lock Upgrade");
                PropertyMergeStrategyCacheLock.EnterWriteLock();
                try
                {
                    strategy = mergeStrategyAttribute.Strategy;
                    PropertyMergeStrategyCache.Add(property, strategy);
                }
                finally
                {
                    Console.Write("Lock Exit");
                    PropertyMergeStrategyCacheLock.ExitWriteLock();
                }
            }

            return strategy;
        }
        finally
        {
            PropertyMergeStrategyCacheLock.ExitUpgradeableReadLock();
        }
    }
}
