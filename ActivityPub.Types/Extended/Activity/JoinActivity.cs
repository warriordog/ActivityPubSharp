// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has joined the object.
/// The target and origin typically have no defined meaning. 
/// </summary>
public class JoinActivity : ASTransitiveActivity
{
    private JoinActivityEntity Entity { get; }

    public JoinActivity() => Entity = new JoinActivityEntity(TypeMap);
    public JoinActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<JoinActivityEntity>();
}

/// <inheritdoc cref="JoinActivity"/>
[ASTypeKey(JoinType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class JoinActivityEntity : ASBase<JoinActivity>
{
    public const string JoinType = "Join";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASActivityEntity.ActivityType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public JoinActivityEntity(TypeMap typeMap) : base(typeMap, JoinType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public JoinActivityEntity() : base(JoinType, ReplacedTypes) {}
}