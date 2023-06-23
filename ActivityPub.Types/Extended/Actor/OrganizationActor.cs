/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;

namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Represents an organization. 
/// </summary>
public class OrganizationActor : ASActor
{
    public const string OrganizationType = "Organization";

    [JsonConstructor]
    public OrganizationActor() : this(OrganizationType) {}

    protected OrganizationActor(string type) : base(type) {}
}