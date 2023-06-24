/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Represents a formal or informal collective of Actors. 
/// </summary>
[ASTypeKey(GroupType)]
public class GroupActor : ASActor
{
    public const string GroupType = "Group";

    [JsonConstructor]
    public GroupActor() : this(GroupType) {}

    protected GroupActor(string type) : base(type) {}
}