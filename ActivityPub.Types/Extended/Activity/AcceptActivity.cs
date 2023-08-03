// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor accepts the object.
/// The target property can be used in certain circumstances to indicate the context into which the object has been accepted. 
/// </summary>
public class AcceptActivity : ASTransitiveActivity
{
    private AcceptActivityEntity Entity { get; }

    public AcceptActivity() => Entity = new AcceptActivityEntity(TypeMap);
    public AcceptActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<AcceptActivityEntity>();
}

/// <inheritdoc cref="AcceptActivity"/>
[ASTypeKey(AcceptType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class AcceptActivityEntity : ASBase<AcceptActivity>
{
    public const string AcceptType = "Accept";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASActivityEntity.ActivityType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public AcceptActivityEntity(TypeMap typeMap) : base(typeMap, AcceptType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public AcceptActivityEntity() : base(AcceptType, ReplacedTypes) {}
}