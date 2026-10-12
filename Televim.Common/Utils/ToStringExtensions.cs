namespace Televim.Common.Utils;

public static class ToStringExtensions
{
    public static string ToPrettyString<T>(this IReadOnlyCollection<T> collection, Func<T, string> itemToString = null)
    {
        return $"[ { string.Join(", ", collection.Select(c => itemToString is null ? c.ToString() : itemToString(c))) } ]";
    }
}
