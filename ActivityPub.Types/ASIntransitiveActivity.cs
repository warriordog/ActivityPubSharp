/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types;

/// <summary>
/// Instances of IntransitiveActivity are a subtype of Activity representing intransitive actions.
/// The object property is therefore inappropriate for these activities. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-intransitiveactivity"/>
[ASTypeKey(IntransitiveActivityType)]
public class ASIntransitiveActivity : ASActivity
{
    public const string IntransitiveActivityType = "IntransitiveActivity";

    [JsonConstructor]
    public ASIntransitiveActivity() : this(IntransitiveActivityType) {}

    protected ASIntransitiveActivity(string type) : base(type) {}
}