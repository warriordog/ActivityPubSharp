/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;

namespace ActivityPub.Types.Extended.Link;

/// <summary>
/// A specialized Link that represents an @mention. 
/// </summary>
public class MentionLink : ASLink
{
    public const string MentionType = "Mention";

    [JsonConstructor]
    public MentionLink() : this(MentionType) {}

    protected MentionLink(string type) : base(type) {}
}