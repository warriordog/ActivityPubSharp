// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.AS;

/// <summary>
///     Base type of all ActivityStreams / ActivityPub objects.
///     Subtypes MUST NOT contain any properties!
///     Instead, all data should be stored in a matching entity class which derives from <see cref="ASEntity{TType}" />.
/// </summary>
/// <remarks>
///     This is a synthetic type created to help adapt ActivityStreams to the .NET object model.
///     It does not exist in the ActivityStreams standard.
/// </remarks>
/// <seealso cref="ASEntity{TType}" />
public abstract class ASType
{
    protected ASType()
    {
        TypeMap = new TypeMap();
        Entity = new ASTypeEntity { TypeMap = TypeMap };
    }

    protected ASType(TypeMap typeMap)
    {
        TypeMap = typeMap;
        Entity = typeMap.AsEntity<ASTypeEntity>();
    }

    private ASTypeEntity Entity { get; }

    /// <summary>
    ///     Type graph that contains this object.
    /// </summary>
    public TypeMap TypeMap { get; }

    /// <summary>
    ///     Populated after deserialization.
    ///     Contains all JSON properties that did not map to any known .NET property.
    /// </summary>
    internal Dictionary<string, JsonElement> UnknownJsonProperties => Entity.UnknownJsonProperties;

    /// <summary>
    ///     Provides the globally unique identifier for an Object or Link.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-id" />
    public string? Id
    {
        get => Entity.Id;
        set => Entity.Id = value;
    }

    /// <summary>
    ///     True if this object is anonymous and should be considered part of its parent context.
    /// </summary>
    /// <remarks>
    ///     Based on https://www.w3.org/TR/activitypub/#obj-id
    /// </remarks>
    [MemberNotNullWhen(false, nameof(Id))]
    public bool IsAnonymous => Entity.IsAnonymous;

    /// <summary>
    ///     Identifies one or more entities to which this object is attributed.
    ///     The attributed entities might not be Actors.
    ///     For instance, an object might be attributed to the completion of another activity.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attributedTo" />
    public LinkableList<ASObject> AttributedTo
    {
        get => Entity.AttributedTo;
        set => Entity.AttributedTo = value;
    }

    /// <summary>
    ///     Identifies an entity that provides a preview of this object.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-preview" />
    public Linkable<ASObject>? Preview
    {
        get => Entity.Preview;
        set => Entity.Preview = value;
    }

    /// <summary>
    ///     A simple, human-readable, plain-text name for the object.
    ///     HTML markup MUST NOT be included.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-name" />
    public NaturalLanguageString? Name
    {
        get => Entity.Name;
        set => Entity.Name = value;
    }

    /// <summary>
    ///     When used on a Link, identifies the MIME media type of the referenced resource.
    ///     When used on an Object, identifies the MIME media type of the value of the content property.
    ///     If not specified, the content property is assumed to contain text/html content.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-mediatype" />
    public string? MediaType
    {
        get => Entity.MediaType;
        set => Entity.MediaType = value;
    }


    /// <inheritdoc cref="TypeMap.IsType{T}()" />
    public bool Is<T>()
        where T : ASType
        => TypeMap.IsType<T>();

    /// <inheritdoc cref="TypeMap.IsType{T}(out T?)" />
    public bool Is<T>([NotNullWhen(true)] out T? instance)
        where T : ASType
        => TypeMap.IsType(out instance);

    /// <inheritdoc cref="TypeMap.AsType{T}" />
    public T As<T>()
        where T : ASType
        => TypeMap.AsType<T>();
}

/// <inheritdoc cref="ASType" />
public sealed class ASTypeEntity : ASEntity<ASType>, ILinkEntity
{
    private string? _id;

    /// <inheritdoc cref="ASType.UnknownJsonProperties" />
    internal Dictionary<string, JsonElement> UnknownJsonProperties { get; } = new();

    /// <inheritdoc cref="ASType.Id" />
    [JsonPropertyName("id")]
    public string? Id
    {
        get => _id;
        set
        {
            if (value == _id)
                return;

            // Cache this for performance
            IsAnonymous = string.IsNullOrWhiteSpace(value);
            _id = value;
        }
    }

    /// <inheritdoc cref="ASType.IsAnonymous" />
    [JsonIgnore]
    [MemberNotNullWhen(false, nameof(Id))]
    public bool IsAnonymous { get; private set; } = true;

    /// <inheritdoc cref="ASType.AttributedTo" />
    [JsonPropertyName("attributedTo")]
    public LinkableList<ASObject> AttributedTo { get; set; } = new();

    /// <inheritdoc cref="ASType.Preview" />
    [JsonPropertyName("preview")]
    public Linkable<ASObject>? Preview { get; set; }

    /// <inheritdoc cref="ASType.Name" />
    [JsonPropertyName("name")]
    public NaturalLanguageString? Name { get; set; }

    /// <inheritdoc cref="ASType.MediaType" />
    [JsonPropertyName("mediaType")]
    public string? MediaType { get; set; }

    public bool RequiresObjectForm => Id != null || AttributedTo.Count != 0 || Preview != null || Name != null || MediaType != null || UnknownJsonProperties.Count != 0;
}