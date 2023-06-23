/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;

namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Represents a service of any kind.
/// </summary>
public class ServiceActor : ASActor
{
    public const string ServiceType = "Service";

    [JsonConstructor]
    public ServiceActor() : this(ServiceType) {}

    protected ServiceActor(string type) : base(type) {}
}