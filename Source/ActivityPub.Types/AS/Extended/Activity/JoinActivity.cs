// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has joined the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class JoinActivity : ASTransitiveActivity, IASModel<JoinActivity, JoinActivityEntity, ASTransitiveActivity>
{
    public const string JoinType = "Join";
    static string IASModel<JoinActivity>.ASTypeName => JoinType;

    public JoinActivity() : this(new TypeMap()) {}

    public JoinActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new JoinActivityEntity();
        TypeMap.Add(Entity);
    }

    public JoinActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public JoinActivity(TypeMap typeMap, JoinActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<JoinActivityEntity>();

    static JoinActivity IASModel<JoinActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private JoinActivityEntity Entity { get; }
}

/// <inheritdoc cref="JoinActivity" />
public sealed class JoinActivityEntity : ASEntity<JoinActivity, JoinActivityEntity> {}