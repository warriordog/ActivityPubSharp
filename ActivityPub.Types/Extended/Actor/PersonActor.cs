/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Represents an individual person. 
/// </summary>
[ASTypeKey(PersonType)]
public class PersonActor : ASActor
{
    public const string PersonType = "Person";

    [JsonConstructor]
    public PersonActor() : this(PersonType) {}

    protected PersonActor(string type) : base(type) {}
}