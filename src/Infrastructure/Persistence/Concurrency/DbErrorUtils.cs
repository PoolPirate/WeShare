using Microsoft.EntityFrameworkCore;
using Npgsql;
using WeShare.Application.Common;

namespace WeShare.Infrastructure.Persistence.Concurrency;
public static class DbErrorUtils
{
    public static DbStatus ParseSaveChangesException(Exception ex, out string? constraintName)
    {
        constraintName = null;
        return ex switch
        {
            DbUpdateConcurrencyException => DbStatus.ConcurrencyEntryDeleted, //Other occurences already handled in the SaveChangesAsync override
            DbUpdateException updateException => ParseDbUpdateException(updateException, out constraintName),
            _ => throw ex,
        };
    }

    private static DbStatus ParseDbUpdateException(DbUpdateException updateException, out string? constraintName)
    {
        if (updateException.InnerException is not PostgresException pEx)
        {
            throw updateException;
        }

        constraintName = pEx.ConstraintName;

        return TryParsePostgresException(pEx, out var result)
            ? result
            : throw updateException;
    }

    private static bool TryParsePostgresException(PostgresException pEx, out DbStatus result)
    {
        result = DbStatus.Success;

        return pEx.SqlState switch
        {
            PostgresErrorCodes.UniqueViolation => TryParseUniqueViolation(pEx.ConstraintName, out result),
            _ => false,
        };
    }

    private static bool TryParseUniqueViolation(string? constraintName, out DbStatus result)
    {
        result = DbStatus.Success;

        if (constraintName is null)
        {
            return false;
        }
        if (constraintName[0] == 'I')
        {
            result = DbStatus.DuplicateIndex;
            return true;
        }
        if (constraintName[0] == 'P')
        {
            result = DbStatus.DuplicatePrimaryKey;
            return true;
        }

        return false;
    }
}
