// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents any kind of event.
/// </summary>
public class EventObject : ASObject, IASModel<EventObject, EventObjectEntity, ASObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Event" types.
    /// </summary>
    public const string EventType = "Event";
    static string IASModel<EventObject>.ASTypeName => EventType;

    /// <inheritdoc />
    public EventObject() => Entity = TypeMap.Extend<EventObjectEntity>();

    /// <inheritdoc />
    public EventObject(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<EventObjectEntity>(isExtending);

    /// <inheritdoc />
    public EventObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public EventObject(TypeMap typeMap, EventObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<EventObjectEntity>();

    static EventObject IASModel<EventObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private EventObjectEntity Entity { get; }
}

/// <inheritdoc cref="EventObject" />
public sealed class EventObjectEntity : ASEntity<EventObject, EventObjectEntity> {}