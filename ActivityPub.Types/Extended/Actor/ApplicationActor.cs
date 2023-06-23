/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;

namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Describes a software application. 
/// </summary>
public class ApplicationActor : ASActor
{
    public const string ApplicationType = "Application";

    [JsonConstructor]
    public ApplicationActor() : this(ApplicationType) {}

    protected ApplicationActor(string type) : base(type) {}
}