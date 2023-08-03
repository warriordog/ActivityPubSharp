// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has read the object. 
/// </summary>
public class ReadActivity : ASTransitiveActivity
{
    private ReadActivityEntity Entity { get; }

    public ReadActivity() => Entity = new ReadActivityEntity(TypeMap);
    public ReadActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ReadActivityEntity>();
}

/// <inheritdoc cref="ReadActivity"/>
[ASTypeKey(ReadType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class ReadActivityEntity : ASBase<ReadActivity>
{
    public const string ReadType = "Read";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASActivityEntity.ActivityType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public ReadActivityEntity(TypeMap typeMap) : base(typeMap, ReadType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public ReadActivityEntity() : base(ReadType, ReplacedTypes) {}
}