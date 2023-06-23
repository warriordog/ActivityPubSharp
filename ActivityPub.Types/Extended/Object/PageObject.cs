/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a Web Page. 
/// </summary>
public class PageObject : DocumentObject
{
    public const string PageType = "Page";

    [JsonConstructor]
    public PageObject() : this(PageType) {}

    protected PageObject(string type) : base(type) {}
}