// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS;

/// <summary>
///     Base type of all ActivityStreams / ActivityPub objects.
///     Subtypes MUST NOT contain any fields or auto-properties!
///     Instead, all data should be stored in a matching entity class which derives from <see cref="ASEntity" />.
/// </summary>
/// <remarks>
///     This is a synthetic type created to help adapt ActivityStreams to the .NET object model.
///     It does not exist in the ActivityStreams standard.
/// </remarks>
/// <seealso cref="ASEntity" />
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature, ImplicitUseTargetFlags.WithInheritors)]
public class ASType : IASModel<ASType, ASTypeEntity>
{
    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public ASType()
    {
        TypeMap = new TypeMap();
        Entity = TypeMap.Extend<ASType, ASTypeEntity>();
    }

    /// <summary>
    ///     Constructs a new instance from an existing type graph.
    ///     The existing graph is either extended or wrapped, depending on the value of <code>isExtending</code>
    /// </summary>
    /// <remarks>
    ///     All overrides MUST call this using <code>base(typeMap, false)</code>
    /// </remarks>
    /// <exception cref="InvalidOperationException">If <code>extendGraph</code> is <see langword="true"/> and the entity type already exists in the graph</exception>
    /// <exception cref="InvalidOperationException">If <code>extendGraph</code> is <see langword="true"/> and the entity requires another entity that is missing from the graph</exception>
    /// <exception cref="InvalidCastException">If <code>extendGraph</code> is <see langword="false"/> and the object is not of type <code>TEntity</code></exception>
    /// <seealso cref="TypeMap.ProjectTo{TModel, TEntity}(bool)" />
    public ASType(TypeMap typeMap, bool isExtending = true)
    {
        TypeMap = typeMap;
        Entity = TypeMap.ProjectTo<ASType, ASTypeEntity>(isExtending);
    }

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public ASType(TypeMap typeMap, ASTypeEntity? entity)
    {
        TypeMap = typeMap;
        Entity = entity ?? typeMap.AsEntity<ASType, ASTypeEntity>();
    }

    static ASType IASModel<ASType>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private ASTypeEntity Entity { get; }

    /// <summary>
    ///     Type graph that contains this object.
    /// </summary>
    public TypeMap TypeMap { get; }

    /// <summary>
    ///     Provides the globally unique identifier for an <code>Object</code> or <code>Link</code>.
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

    /// <inheritdoc cref="Types.TypeMap.IsModel{TModel}()" />
    public bool Is<TModel>()
        where TModel : ASType, IASModel<TModel>
        => TypeMap.IsModel<TModel>();

    /// <inheritdoc cref="Types.TypeMap.IsModel{TModel}(out TModel)" />
    public bool Is<TModel>([NotNullWhen(true)] out TModel? instance)
        where TModel : ASType, IASModel<TModel>
        => TypeMap.IsModel(out instance);

    /// <inheritdoc cref="Types.TypeMap.AsModel{TModel}" />
    public TModel As<TModel>()
        where TModel : ASType, IASModel<TModel>
        => TypeMap.AsModel<TModel>();
}

/// <inheritdoc cref="ASType" />
public sealed class ASTypeEntity : ASEntity<ASType, ASTypeEntity>
{
    private string? _id;

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

    /// <inheritdoc />
    public override bool RequiresObjectForm => Id != null || AttributedTo.Count != 0 || Preview != null || Name != null || MediaType != null;
}