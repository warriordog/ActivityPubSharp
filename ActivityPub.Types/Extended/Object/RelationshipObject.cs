// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Describes a relationship between two individuals.
/// The subject and object properties are used to identify the connected individuals.
/// </summary>
[ASTypeKey(RelationshipType)]
public class RelationshipObject : ASObject
{
    public const string RelationshipType = "Relationship";

    [JsonConstructor]
    public RelationshipObject() : this(RelationshipType) {}

    protected RelationshipObject(string type) : base(type) {}

    /// <summary>
    /// Describes the entity to which the subject is related. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object"/>
    [JsonPropertyName("object")]
    public LinkableList<ASObject> Object { get; set; } = new();

    /// <summary>
    /// On a Relationship object, the subject property identifies one of the connected individuals.
    /// For instance, for a Relationship object describing "John is related to Sally", subject would refer to John. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-subject"/>
    [JsonPropertyName("subject")]
    public Linkable<ASObject>? Subject { get; set; }

    /// <summary>
    /// On a Relationship object, the relationship property identifies the kind of relationship that exists between subject and object. 
    /// </summary>
    /// <remarks>
    /// This is supposed to be Range = "Object", but all the examples have a string URL
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-relationship"/>
    [JsonPropertyName("relationship")]
    public Linkable<ASObject>? Relationship { get; set; }
}