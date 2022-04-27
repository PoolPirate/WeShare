namespace WeShare.Domain.Common;

/// <summary>
/// A attribute for selecting the strategy to use when merging properties
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class MergeStrategyAttribute : Attribute
{
    /// <summary>
    /// The selected strategy to use while merging.
    /// </summary>
    public MergeStrategy Strategy { get; }

    /// <summary>
    /// Creates a new MergeStrategyAttribute.
    /// </summary>
    /// <param name="strategy"></param>
    public MergeStrategyAttribute(MergeStrategy strategy)
    {
        Strategy = strategy;
    }
}
