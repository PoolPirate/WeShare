namespace WeShare.Extensions;

public static partial class Extensions
{
    public static bool ContainsAny<T>(this IEnumerable<T> current, IEnumerable<T> other)
    {
        foreach (var value in other)
        {
            if (current.Contains(value))
            {
                return true;
            }
        }

        return false;
    }
}
