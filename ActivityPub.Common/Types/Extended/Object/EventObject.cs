namespace ActivityPub.Common.Types.Extended.Object;

/// <summary>
/// Represents any kind of event.
/// </summary>
public class EventObject : ASObject
{
    public EventObject(string type = "Event") : base(type) {}
}