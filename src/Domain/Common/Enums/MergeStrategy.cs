namespace WeShare.Domain.Common;

/// <summary>
/// Strategies for merging conflicting property changes.
/// </summary>
public enum MergeStrategy
{
    /// <summary>
    /// Take the smaller of both values.
    /// </summary>
    TakeLargest,
    /// <summary>
    /// Take the larger of both values.
    /// </summary>
    TakeSmallest,
    /// <summary>
    /// Add up the relative changes, add those to the original value and take that as new value.
    /// </summary>
    AddChanges,
    /// <summary>
    /// Take the current value and discard the other one.
    /// </summary>
    TakeCurrent,
    /// <summary>
    /// Take the other value and discard the current one.
    /// </summary>
    TakeOther,
    /// <summary>
    /// Take the value that was stored before any changes have been made.
    /// </summary>
    TakeOriginal
}
