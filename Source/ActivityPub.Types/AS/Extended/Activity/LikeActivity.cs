// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor likes, recommends or endorses the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class LikeActivity : ASTransitiveActivity, IASModel<LikeActivity, LikeActivityEntity, ASTransitiveActivity>
{
    public const string LikeType = "Like";
    static string IASModel<LikeActivity>.ASTypeName => LikeType;

    public LikeActivity() : this(new TypeMap()) {}

    public LikeActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new LikeActivityEntity();
        TypeMap.AddEntity(Entity);
    }

    public LikeActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public LikeActivity(TypeMap typeMap, LikeActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<LikeActivityEntity>();

    static LikeActivity IASModel<LikeActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private LikeActivityEntity Entity { get; }
}

/// <inheritdoc cref="LikeActivity" />
public sealed class LikeActivityEntity : ASEntity<LikeActivity, LikeActivityEntity> {}