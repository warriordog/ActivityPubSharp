// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is calling the target's attention the object.
///     The origin typically has no defined meaning.
/// </summary>
public class AnnounceActivity : ASTransitiveActivity, IASModel<AnnounceActivity, AnnounceActivityEntity, ASTransitiveActivity>
{
    public const string AnnounceType = "Announce";
    static string IASModel<AnnounceActivity>.ASTypeName => AnnounceType;

    public AnnounceActivity() : this(new TypeMap()) {}

    public AnnounceActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new AnnounceActivityEntity();
        TypeMap.Add(Entity);
    }

    public AnnounceActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public AnnounceActivity(TypeMap typeMap, AnnounceActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<AnnounceActivityEntity>();

    static AnnounceActivity IASModel<AnnounceActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private AnnounceActivityEntity Entity { get; }
}

/// <inheritdoc cref="AnnounceActivity" />
public sealed class AnnounceActivityEntity : ASEntity<AnnounceActivity, AnnounceActivityEntity> {}