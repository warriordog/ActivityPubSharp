// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents any kind of event.
/// </summary>
public class EventObject : ASObject
{
    private EventObjectEntity Entity { get; }

    public EventObject() => Entity = new EventObjectEntity(TypeMap);
    public EventObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<EventObjectEntity>();
}

/// <inheritdoc cref="EventObject"/>
[ASTypeKey(EventType)]
[ImpliesOtherEntity(typeof(ASObjectEntity))]
public sealed class EventObjectEntity : ASBase<EventObject>
{
    public const string EventType = "Event";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASObjectEntity.ObjectType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string?,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public EventObjectEntity(TypeMap typeMap) : base(typeMap, EventType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string?, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public EventObjectEntity() : base(EventType, ReplacedTypes) {}
}