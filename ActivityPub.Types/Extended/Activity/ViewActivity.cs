/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has viewed the object. 
/// </summary>
public class ViewActivity : ASTransitiveActivity
{
    public const string ViewType = "View";
    public ViewActivity(string type = ViewType) : base(type) {}
}