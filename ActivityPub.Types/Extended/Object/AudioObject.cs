/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents an audio document of any kind. 
/// </summary>
public class AudioObject : DocumentObject
{
    public const string AudioType = "Audio";

    [JsonConstructor]
    public AudioObject() : this(AudioType) {}

    protected AudioObject(string type) : base(type) {}
}