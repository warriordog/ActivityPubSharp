// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

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
public sealed class JoinActivityEntity : ASBase
{
    public const string JoinType = "Join";

    public JoinActivityEntity(TypeMap typeMap) : base(JoinType, typeMap) {}
}