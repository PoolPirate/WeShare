namespace WeShare.Application.Common.Exceptions;
public class UnhandledDbStatusException : Exception
{
    public UnhandledDbStatusException(DbSaveResult saveResult)
        : base($"Unhandled DB Status while saving: Status={saveResult.Status}")
    {

    }
}

