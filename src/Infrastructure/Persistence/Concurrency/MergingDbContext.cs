using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using WeShare.Application.Common;

namespace WeShare.Infrastructure.Persistence.Concurrency;
public abstract class MergingDbContext : DbContext
{
    private readonly Random Random;
    private readonly PropertyMerger PropertyMerger;

    protected MergingDbContext(DbContextOptions options, Random random, PropertyMerger propertyMerger)
        : base(options)
    {
        Random = random;
        PropertyMerger = propertyMerger;
    }

    public virtual async Task<DbSaveResult> SaveChangesAsync(DbStatus allowedStatuses = DbStatus.Success, bool discardConcurrentDeletedEntries = false,
        IDbContextTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        try
        {
            int affectedRows = await SaveChangesAsync(discardConcurrentDeletedEntries, cancellationToken);
            return DbSaveResult.FromSuccess(affectedRows);
        }
        catch (Exception ex)
        {
            var status = DbErrorUtils.ParseSaveChangesException(ex, out string? constraintName);

            if (!allowedStatuses.HasFlag(status))
            {
                throw;
            }

            return DbSaveResult.FromError(status, constraintName);
        }
    }

    public override async Task<int> SaveChangesAsync(bool discardConcurrentDeletedEntries, CancellationToken cancellationToken = default)
    {
        bool saved = false;
        while (!saved)
        {
            try
            {
                return await base.SaveChangesAsync(true, cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await MergeConcurrencyConflicts(ex.Entries, discardConcurrentDeletedEntries, cancellationToken))
                {
                    throw;
                }

                await StepBackBeforeRetry(cancellationToken);
            }
        }

        throw new InvalidOperationException();
    }

    private Task StepBackBeforeRetry(CancellationToken cancellationToken) => Task.Delay(Random.Next(1, 20), cancellationToken);

    private async Task<bool> MergeConcurrencyConflicts(IEnumerable<EntityEntry> conflicts, bool discardConcurrentDeletedEntries, CancellationToken cancellationToken)
    {
        foreach (var entry in conflicts)
        {
            var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);

            if (databaseValues is null)
            {
                if (!discardConcurrentDeletedEntries)
                {
                    return false;
                }

                entry.State = EntityState.Detached;
                continue;
            }

            var originalValues = entry.OriginalValues;
            var proposedValues = entry.CurrentValues;

            foreach (var property in proposedValues.Properties)
            {
                var propertyEntry = entry.Property(property.Name);
                if (!propertyEntry.IsModified || propertyEntry.IsTemporary)
                {
                    continue;
                }
                if (property.PropertyInfo == null)
                {
                    continue;
                }

                object? originalValue = originalValues[property];
                object? proposedValue = proposedValues[property];
                object? databaseValue = databaseValues[property];

                if (Equals(originalValue, databaseValue))
                {
                    continue;
                }

                Trace.Assert(originalValue != null, "Original value null");
                Trace.Assert(proposedValue != null, "Proposed value null");
                Trace.Assert(databaseValue != null, "Database value null");

                object mergedValue = PropertyMerger.Merge(property.PropertyInfo, originalValue!, proposedValue!, databaseValue!);

                proposedValues[property] = mergedValue;
            }

            entry.OriginalValues.SetValues(databaseValues);
        }

        return true;
    }
}
