namespace WeShare.Application.Common;

[Flags]
public enum DbStatus : byte
{
    Success = 1,
    DuplicatePrimaryKey = 2,
    DuplicateIndex = 4,
    ConcurrencyEntryDeleted = 8,
}
