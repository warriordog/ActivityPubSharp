// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is removing the object.
/// If specified, the origin indicates the context from which the object is being removed. 
/// </summary>
public class RemoveActivity : ASTargetedActivity
{
    private RemoveActivityEntity Entity { get; }

    public RemoveActivity() => Entity = new RemoveActivityEntity(TypeMap);
    public RemoveActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<RemoveActivityEntity>();
}

/// <inheritdoc cref="RemoveActivity"/>
[ASTypeKey(RemoveType)]
[ImpliesOtherEntity(typeof(ASTargetedActivityEntity))]
public sealed class RemoveActivityEntity : ASBase<RemoveActivity>
{
    public const string RemoveType = "Remove";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASActivityEntity.ActivityType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public RemoveActivityEntity(TypeMap typeMap) : base(typeMap, RemoveType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public RemoveActivityEntity() : base(RemoveType, ReplacedTypes) {}
}