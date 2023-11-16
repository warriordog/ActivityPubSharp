// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor dislikes the object.
/// </summary>
public class DislikeActivity : ASTransitiveActivity, IASModel<DislikeActivity, DislikeActivityEntity, ASTransitiveActivity>
{
    public const string DislikeType = "Dislike";
    static string IASModel<DislikeActivity>.ASTypeName => DislikeType;

    public DislikeActivity() : this(new TypeMap()) {}

    public DislikeActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new DislikeActivityEntity();
        TypeMap.Add(Entity);
    }

    public DislikeActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public DislikeActivity(TypeMap typeMap, DislikeActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<DislikeActivityEntity>();

    static DislikeActivity IASModel<DislikeActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private DislikeActivityEntity Entity { get; }
}

/// <inheritdoc cref="DislikeActivity" />
public sealed class DislikeActivityEntity : ASEntity<DislikeActivity, DislikeActivityEntity> {}