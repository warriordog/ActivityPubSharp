// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is "flagging" the object.
///     Flagging is defined in the sense common to many social platforms as reporting content as being inappropriate for any number of reasons.
/// </summary>
public class FlagActivity : ASTransitiveActivity, IASModel<FlagActivity, FlagActivityEntity, ASTransitiveActivity>
{
    public const string FlagType = "Flag";
    static string IASModel<FlagActivity>.ASTypeName => FlagType;

    public FlagActivity() : this(new TypeMap()) {}

    public FlagActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new FlagActivityEntity();
        TypeMap.AddEntity(Entity);
    }

    public FlagActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public FlagActivity(TypeMap typeMap, FlagActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<FlagActivityEntity>();

    static FlagActivity IASModel<FlagActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private FlagActivityEntity Entity { get; }
}

/// <inheritdoc cref="FlagActivity" />
public sealed class FlagActivityEntity : ASEntity<FlagActivity, FlagActivityEntity> {}