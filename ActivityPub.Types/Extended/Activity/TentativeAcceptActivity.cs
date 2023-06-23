/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// A specialization of Accept indicating that the acceptance is tentative.
/// </summary>
public class TentativeAcceptActivity : AcceptActivity
{
    public const string TentativeAcceptType = "TentativeAccept";

    [JsonConstructor]
    public TentativeAcceptActivity() : this(TentativeAcceptType) {}

    protected TentativeAcceptActivity(string type) : base(type) {}
}