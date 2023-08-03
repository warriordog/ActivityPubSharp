// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Describes a relationship between two individuals.
/// The subject and object properties are used to identify the connected individuals.
/// </summary>
public class RelationshipObject : ASObject
{
    private RelationshipObjectEntity Entity { get; }


    public RelationshipObject() => Entity = new RelationshipObjectEntity(TypeMap);
    public RelationshipObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<RelationshipObjectEntity>();

    /// <summary>
    /// Describes the entity to which the subject is related. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object"/>
    public LinkableList<ASObject> Object
    {
        get => Entity.Object;
        set => Entity.Object = value;
    }

    /// <summary>
    /// On a Relationship object, the subject property identifies one of the connected individuals.
    /// For instance, for a Relationship object describing "John is related to Sally", subject would refer to John. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-subject"/>
    public Linkable<ASObject>? Subject
    {
        get => Entity.Subject;
        set => Entity.Subject = value;
    }

    /// <summary>
    /// On a Relationship object, the relationship property identifies the kind of relationship that exists between subject and object. 
    /// </summary>
    /// <remarks>
    /// This is supposed to be Range = "Object", but all the examples have a string URL
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-relationship"/>
    public Linkable<ASObject>? Relationship
    {
        get => Entity.Relationship;
        set => Entity.Relationship = value;
    }
}

/// <inheritdoc cref="RelationshipObject"/>
[ASTypeKey(RelationshipType)]
[ImpliesOtherEntity(typeof(ASObjectEntity))]
public sealed class RelationshipObjectEntity : ASBase<RelationshipObject>
{
    public const string RelationshipType = "Relationship";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASObjectEntity.ObjectType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public RelationshipObjectEntity(TypeMap typeMap) : base(typeMap, RelationshipType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public RelationshipObjectEntity() : base(RelationshipType, ReplacedTypes) {}

    /// <inheritdoc cref="RelationshipObject.Object"/>
    [JsonPropertyName("object")]
    public LinkableList<ASObject> Object { get; set; } = new();

    /// <inheritdoc cref="RelationshipObject.Subject"/>
    [JsonPropertyName("subject")]
    public Linkable<ASObject>? Subject { get; set; }

    /// <inheritdoc cref="RelationshipObject.Relationship"/>
    [JsonPropertyName("relationship")]
    public Linkable<ASObject>? Relationship { get; set; }
}