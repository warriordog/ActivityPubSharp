// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is "following" the object.
/// Following is defined in the sense typically used within Social systems in which the actor is interested in any activity performed by or on the object.
/// The target and origin typically have no defined meaning. 
/// </summary>
public class FollowActivity : ASTransitiveActivity
{
    private FollowActivityEntity Entity { get; }

    public FollowActivity() => Entity = new FollowActivityEntity(TypeMap);
    public FollowActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<FollowActivityEntity>();
}

/// <inheritdoc cref="FollowActivity"/>
[ASTypeKey(FollowType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class FollowActivityEntity : ASBase<FollowActivity>
{
    public const string FollowType = "Follow";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASActivityEntity.ActivityType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public FollowActivityEntity(TypeMap typeMap) : base(typeMap, FollowType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public FollowActivityEntity() : base(FollowType, ReplacedTypes) {}
}