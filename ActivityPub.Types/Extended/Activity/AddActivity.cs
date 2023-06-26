/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has added the object to the target.
/// If the target property is not explicitly specified, the target would need to be determined implicitly by context.
/// The origin can be used to identify the context from which the object originated. 
/// </summary>
[ASTypeKey(AddType)]
public class AddActivity : ASTargetedActivity
{
    public const string AddType = "Add";

    [JsonConstructor]
    public AddActivity() : this(AddType) {}

    protected AddActivity(string type) : base(type) {}
}