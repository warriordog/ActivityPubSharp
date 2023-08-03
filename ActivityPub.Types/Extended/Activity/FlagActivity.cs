// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is "flagging" the object.
/// Flagging is defined in the sense common to many social platforms as reporting content as being inappropriate for any number of reasons. 
/// </summary>
public class FlagActivity : ASTransitiveActivity
{
    private FlagActivityEntity Entity { get; }

    public FlagActivity() => Entity = new FlagActivityEntity(TypeMap);
    public FlagActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<FlagActivityEntity>();
}

/// <inheritdoc cref="FlagActivity"/>
[ASTypeKey(FlagType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class FlagActivityEntity : ASBase<FlagActivity>
{
    public const string FlagType = "Flag";

    /// <inheritdoc cref="ASBase{T}(string?, TypeMap)"/>
    public FlagActivityEntity(TypeMap typeMap) : base(FlagType, typeMap) {}

    /// <inheritdoc cref="ASBase{T}(string?)"/>
    [JsonConstructor]
    public FlagActivityEntity() : base(FlagType) {}
}