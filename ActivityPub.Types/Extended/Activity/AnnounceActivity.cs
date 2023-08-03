// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is calling the target's attention the object.
/// The origin typically has no defined meaning. 
/// </summary>
public class AnnounceActivity : ASTransitiveActivity
{
    private AnnounceActivityEntity Entity { get; }

    public AnnounceActivity() => Entity = new AnnounceActivityEntity(TypeMap);
    public AnnounceActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<AnnounceActivityEntity>();
}

/// <inheritdoc cref="AnnounceActivity"/>
[ASTypeKey(AnnounceType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class AnnounceActivityEntity : ASBase<AnnounceActivity>
{
    public const string AnnounceType = "Announce";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASActivityEntity.ActivityType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public AnnounceActivityEntity(TypeMap typeMap) : base(typeMap, AnnounceType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public AnnounceActivityEntity() : base(AnnounceType, ReplacedTypes) {}
}