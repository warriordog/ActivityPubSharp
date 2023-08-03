// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has created the object.
/// </summary>
public class CreateActivity : ASTransitiveActivity
{
    private CreateActivityEntity Entity { get; }

    public CreateActivity() => Entity = new CreateActivityEntity(TypeMap);
    public CreateActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<CreateActivityEntity>();
}

/// <inheritdoc cref="CreateActivity"/>
[ASTypeKey(CreateType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class CreateActivityEntity : ASBase<CreateActivity>
{
    public const string CreateType = "Create";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASActivityEntity.ActivityType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public CreateActivityEntity(TypeMap typeMap) : base(typeMap, CreateType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public CreateActivityEntity() : base(CreateType, ReplacedTypes) {}
}