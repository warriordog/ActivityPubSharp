// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has viewed the object.
/// </summary>
public class ViewActivity : ASTransitiveActivity, IASModel<ViewActivity, ViewActivityEntity, ASTransitiveActivity>
{
    public const string ViewType = "View";
    static string IASModel<ViewActivity>.ASTypeName => ViewType;

    public ViewActivity() : this(new TypeMap()) {}

    public ViewActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new ViewActivityEntity();
        TypeMap.Add(Entity);
    }

    public ViewActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public ViewActivity(TypeMap typeMap, ViewActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ViewActivityEntity>();

    static ViewActivity IASModel<ViewActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ViewActivityEntity Entity { get; }
}

/// <inheritdoc cref="ViewActivity" />
public sealed class ViewActivityEntity : ASEntity<ViewActivity, ViewActivityEntity> {}