// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents any kind of event.
/// </summary>
public class EventObject : ASObject
{
    public EventObject() => Entity = new EventObjectEntity { TypeMap = TypeMap };
    public EventObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<EventObjectEntity>();
    private EventObjectEntity Entity { get; }
}

/// <inheritdoc cref="EventObject" />
[APConvertible(EventType)]
[ImpliesOtherEntity(typeof(ASObjectEntity))]
public sealed class EventObjectEntity : ASEntity<EventObject>
{
    public const string EventType = "Event";
    public override string ASTypeName => EventType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASObjectEntity.ObjectType
    };
}