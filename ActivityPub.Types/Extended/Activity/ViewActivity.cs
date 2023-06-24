/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has viewed the object. 
/// </summary>
[ASTypeKey(ViewType)]
public class ViewActivity : ASTransitiveActivity
{
    public const string ViewType = "View";

    [JsonConstructor]
    public ViewActivity() : this(ViewType) {}

    protected ViewActivity(string type) : base(type) {}
}