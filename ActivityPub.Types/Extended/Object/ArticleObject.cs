/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents any kind of multi-paragraph written work.
/// </summary>
public class ArticleObject : ASObject
{
    public const string ArticleType = "Article";

    [JsonConstructor]
    public ArticleObject() : this(ArticleType) {}

    protected ArticleObject(string type) : base(type) {}
}