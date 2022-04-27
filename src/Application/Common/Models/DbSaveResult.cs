namespace WeShare.Application.Common;

public class DbSaveResult
{
    private const string IndexNamePrefix = "IX_";
    private const string IndexNamePropertySeparator = "_";

    public DbStatus Status { get; }
    public int AffectedRows { get; }

    public string? ConstraintName { get; }

    public static DbSaveResult FromSuccess(int affectedRows) => new DbSaveResult(DbStatus.Success, affectedRows, null);

    public static DbSaveResult FromError(DbStatus status, string? constraintName) => new DbSaveResult(status, 0, constraintName);

    private DbSaveResult(DbStatus status, int affectedRows, string? constraintName)
    {
        Status = status;
        AffectedRows = affectedRows;
        ConstraintName = constraintName;
    }

    public bool IsConflictingIndex(Type entitiyType, params string[] propertyNames)
    {
        if (String.IsNullOrWhiteSpace(ConstraintName))
        {
            return false;
        }

        var constraint = ConstraintName.AsSpan();
        if (!constraint.StartsWith(IndexNamePrefix))
        {
            return false;
        }

        constraint = constraint[IndexNamePrefix.Length..];

        if (!constraint.StartsWith(entitiyType.Name, StringComparison.Ordinal))
        {
            return false;
        }

        constraint = constraint[entitiyType.Name.Length..];

        foreach (string propertyName in propertyNames)
        {
            if (!constraint.StartsWith(IndexNamePropertySeparator))
            {
                return false;
            }

            constraint = constraint[IndexNamePropertySeparator.Length..];

            if (!constraint.StartsWith(propertyName, StringComparison.Ordinal))
            {
                return false;
            }

            constraint = constraint[propertyName.Length..];
        }

        return constraint.Length == 0;
    }
}
