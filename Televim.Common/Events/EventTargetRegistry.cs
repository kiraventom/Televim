namespace Televim.Common.Events;

public class EventTargetRegistry
{
    private readonly HashSet<Type> _ignoredByDefault = new();

    public IReadOnlySet<Type> IgnoredByDefault => _ignoredByDefault;

    public bool AddIgnoredByDefault(Type targetType) => _ignoredByDefault.Add(targetType);
}
