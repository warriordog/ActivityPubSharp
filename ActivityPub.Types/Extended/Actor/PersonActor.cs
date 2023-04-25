/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Represents an individual person. 
/// </summary>
public class PersonActor : ASActor
{
    public const string PersonType = "Person";
    public PersonActor(string type = PersonType) : base(type) {}
}