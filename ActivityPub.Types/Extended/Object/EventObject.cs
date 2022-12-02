namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents any kind of event.
/// </summary>
public class EventObject : ASObject
{
    public const string EventType = "Event";
    public EventObject(string type = EventType) : base(type) {}
}