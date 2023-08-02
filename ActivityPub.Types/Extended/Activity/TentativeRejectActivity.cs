// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// A specialization of Reject in which the rejection is considered tentative.
/// </summary>
public class TentativeRejectActivity : RejectActivity
{
    private TentativeRejectActivityEntity Entity { get; }
    
    public TentativeRejectActivity() => Entity = new TentativeRejectActivityEntity(TypeMap);
    public TentativeRejectActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<TentativeRejectActivityEntity>();
}


/// <inheritdoc cref="TentativeRejectActivity"/>
[ASTypeKey(TentativeRejectType)]
public sealed class TentativeRejectActivityEntity : ASBase
{
    public const string TentativeRejectType = "TentativeReject";

        /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public TentativeRejectActivityEntity(TypeMap typeMap) : base(TentativeRejectType, typeMap) {}
    
    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public TentativeRejectActivityEntity() : base(TentativeRejectType) {}
}