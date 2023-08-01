// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has listened to the object. 
/// </summary>
public class ListenActivity : ASTransitiveActivity
{
    private ListenActivityEntity Entity { get; }
    
    public ListenActivity() => Entity = new ListenActivityEntity(TypeMap);
    public ListenActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ListenActivityEntity>();
}


/// <inheritdoc cref="ListenActivity"/>
[ASTypeKey(ListenType)]
public sealed class ListenActivityEntity : ASBase
{
    public const string ListenType = "Listen";

    public ListenActivityEntity(TypeMap typeMap) : base(ListenType, typeMap) {}
}