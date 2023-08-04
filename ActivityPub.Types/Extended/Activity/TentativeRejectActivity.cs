// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// A specialization of Reject in which the rejection is considered tentative.
/// </summary>
[APType(TentativeRejectType)]
public class TentativeRejectActivity : RejectActivity
{
    public const string TentativeRejectType = "TentativeReject";

    [JsonConstructor]
    public TentativeRejectActivity() : this(TentativeRejectType) {}

    protected TentativeRejectActivity(string type) : base(type) {}
}
