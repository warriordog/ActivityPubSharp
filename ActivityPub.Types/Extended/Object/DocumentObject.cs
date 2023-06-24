/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a document of any kind. 
/// </summary>
[ASTypeKey(DocumentType)]
public class DocumentObject : ASObject
{
    public const string DocumentType = "Document";

    [JsonConstructor]
    public DocumentObject() : this(DocumentType) {}

    protected DocumentObject(string type) : base(type) {}
}