/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has read the object. 
/// </summary>
[ASTypeKey(ReadType)]
public class ReadActivity : ASTransitiveActivity
{
    public const string ReadType = "Read";

    [JsonConstructor]
    public ReadActivity() : this(ReadType) {}

    protected ReadActivity(string type) : base(type) {}
}