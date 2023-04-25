/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has created the object.
/// </summary>
public class CreateActivity : ASTransitiveActivity
{
    public const string CreateType = "Create";
    public CreateActivity(string type = CreateType) : base(type) {}
}