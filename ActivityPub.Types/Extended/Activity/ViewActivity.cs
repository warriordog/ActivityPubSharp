// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has viewed the object. 
/// </summary>
public class ViewActivity : ASTransitiveActivity
{
    private ViewActivityEntity Entity { get; }

    public ViewActivity() => Entity = new ViewActivityEntity(TypeMap);
    public ViewActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ViewActivityEntity>();
}

/// <inheritdoc cref="ViewActivity"/>
[ASTypeKey(ViewType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class ViewActivityEntity : ASBase<ViewActivity>
{
    public const string ViewType = "View";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASActivityEntity.ActivityType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string?,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public ViewActivityEntity(TypeMap typeMap) : base(typeMap, ViewType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string?, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public ViewActivityEntity() : base(ViewType, ReplacedTypes) {}
}