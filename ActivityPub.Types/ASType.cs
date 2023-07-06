// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// Base type of all ActivityStreams / ActivityPub objects.
/// </summary>
/// <remarks>
/// This is a synthetic type created to help adapt ActivityStreams to the .NET object model.
/// It does not exist in the ActivityStreams standard.
/// </remarks>
public abstract class ASType : IJsonConvertible<ASType>
{
    protected ASType(string defaultType) => Types.Add(defaultType);

    /// <summary>
    /// Identifies the Object or Link types.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-type"/>
    [JsonPropertyName("type")]
    [JsonConverter(typeof(ListableConverter))]
    public HashSet<string> Types { get; set; } = new();

    /// <summary>
    /// Lists the JSON-LD contexts used by this object.
    /// Should be a full URL
    /// </summary>
    [JsonPropertyName("@context")]
    [JsonConverter(typeof(ListableConverter))]
    public HashSet<string> JsonLdContexts { get; set; } = new()
    {
        // We always need the base ActivityStreams context
        "https://www.w3.org/ns/activitystreams"
    };

    /// <summary>
    /// Provides the globally unique identifier for an Object or Link.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-id"/>
    public string? Id
    {
        get => _id;
        set
        {
            if (value == _id) return;

            // Cache this for performance
            IsAnonymous = string.IsNullOrWhiteSpace(value);
            _id = value;
        }
    }

    private string? _id;

    /// <summary>
    /// True if this object is anonymous and should be considered part of its parent context.
    /// </summary>
    /// <remarks>
    /// Based on https://www.w3.org/TR/activitypub/#obj-id
    /// </remarks>
    [JsonIgnore]
    [MemberNotNullWhen(false, nameof(Id))]
    public bool IsAnonymous { get; private set; } = true;

    /// <summary>
    /// Identifies one or more entities to which this object is attributed.
    /// The attributed entities might not be Actors.
    /// For instance, an object might be attributed to the completion of another activity. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attributedTo"/>
    public LinkableList<ASObject> AttributedTo { get; set; } = new();

    /// <summary>
    /// Identifies an entity that provides a preview of this object. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-preview"/>
    public Linkable<ASObject>? Preview { get; set; }

    /// <summary>
    /// A simple, human-readable, plain-text name for the object.
    /// HTML markup MUST NOT be included.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-name"/>
    public NaturalLanguageString? Name { get; set; }

    /// <summary>
    /// When used on a Link, identifies the MIME media type of the referenced resource.
    /// When used on an Object, identifies the MIME media type of the value of the content property.
    /// If not specified, the content property is assumed to contain text/html content. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-mediaType"/>
    public Linkable<ASObject>? MediaType { get; set; }


    protected virtual void ReadJson(JsonElement element, JsonOptions options)
    {
        if (element.TryGetProperty("type", out var type))
            Types = type.Deserialize<HashSet<string>>(options.SerializerOptions)!;

        if (element.TryGetProperty("@context", out var ldContext))
            JsonLdContexts = ldContext.Deserialize<HashSet<string>>(options.SerializerOptions)!;

        if (element.TryGetProperty("id", out var id) && id.TryGetString(out var idStr))
            Id = idStr;

        if (element.TryGetProperty("attributedTo", out var attributedTo))
            AttributedTo = attributedTo.Deserialize<LinkableList<ASObject>>(options.SerializerOptions)!;

        if (element.TryGetProperty("preview", out var preview))
            Preview = preview.Deserialize<Linkable<ASObject>>(options.SerializerOptions)!;

        if (element.TryGetProperty("name", out var name))
            Name = name.Deserialize<NaturalLanguageString>(options.SerializerOptions)!;

        if (element.TryGetProperty("mediaType", out var mediaType))
            MediaType = mediaType.Deserialize<Linkable<ASObject>>(options.SerializerOptions)!;
    }

    protected virtual void WriteJson(JsonNode node, JsonOptions options)
    {
        node["type"] = JsonSerializer.SerializeToNode(Types, options.SerializerOptions);
        node["@context"] = JsonSerializer.SerializeToNode(JsonLdContexts, options.SerializerOptions);
        if (!IsAnonymous)
            node["id"] = JsonValue.Create(Id, options.NodeOptions);
        if (AttributedTo.Count > 0)
            node["attributedTo"] = JsonSerializer.SerializeToNode(AttributedTo, options.SerializerOptions);
        if (Preview != null)
            node["preview"] = JsonSerializer.SerializeToNode(Preview, options.SerializerOptions);
        if (Name != null)
            node["name"] = JsonSerializer.SerializeToNode(Name, options.SerializerOptions);
        if (MediaType != null)
            node["mediaType"] = JsonSerializer.SerializeToNode(MediaType, options.SerializerOptions);
    }

    public static ASType Deserialize(JsonElement element, JsonOptions options) => throw new NotSupportedException("ASType is abstract and should be deserialized through a subclass");
    public static JsonNode Serialize(ASType obj, JsonOptions options) => throw new NotSupportedException("ASType is abstract and should be serialized through a subclass");
}