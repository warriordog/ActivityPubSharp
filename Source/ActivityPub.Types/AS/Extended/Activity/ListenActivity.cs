// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has listened to the object.
/// </summary>
public class ListenActivity : ASTransitiveActivity, IASModel<ListenActivity, ListenActivityEntity, ASTransitiveActivity>
{
    public const string ListenType = "Listen";
    static string IASModel<ListenActivity>.ASTypeName => ListenType;

    public ListenActivity() : this(new TypeMap()) {}

    public ListenActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<ListenActivityEntity>();

    public ListenActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public ListenActivity(TypeMap typeMap, ListenActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ListenActivityEntity>();

    static ListenActivity IASModel<ListenActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ListenActivityEntity Entity { get; }
}

/// <inheritdoc cref="ListenActivity" />
public sealed class ListenActivityEntity : ASEntity<ListenActivity, ListenActivityEntity> {}