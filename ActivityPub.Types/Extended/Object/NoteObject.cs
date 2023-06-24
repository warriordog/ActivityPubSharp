/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a short written work typically less than a single paragraph in length. 
/// </summary>
[ASTypeKey(NoteType)]
public class NoteObject : ASObject
{
    public const string NoteType = "Note";

    [JsonConstructor]
    public NoteObject() : this(NoteType) {}

    protected NoteObject(string type) : base(type) {}
}