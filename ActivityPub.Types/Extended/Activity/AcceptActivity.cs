/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor accepts the object.
/// The target property can be used in certain circumstances to indicate the context into which the object has been accepted. 
/// </summary>
public class AcceptActivity : ASTransitiveActivity
{
    public const string AcceptType = "Accept";

    [JsonConstructor]
    public AcceptActivity() : this(AcceptType) {}

    protected AcceptActivity(string type) : base(type) {}
}