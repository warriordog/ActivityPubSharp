// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ActivityPub.Types.AS.Collection;
using ActivityPub.Types.AS.Extended.Object;
using ActivityPub.Types.Util;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS;

/// <summary>
///     Describes an object of any kind.
///     The Object type serves as the base type for most of the other kinds of objects defined in the Activity Vocabulary, including other Core types such as Activity, IntransitiveActivity, Collection and OrderedCollection.
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object" />
public class ASObject : ASType, IASModel<ASObject, ASObjectEntity, ASType>
{
    /// <summary>
    ///     ActivityStreams type name for "Object" types.
    /// </summary>
    [PublicAPI]
    public const string ObjectType = "Object";
    static string IASModel<ASObject>.ASTypeName => ObjectType;

    /// <inheritdoc />
    public ASObject() => Entity = TypeMap.Extend<ASObject, ASObjectEntity>();

    /// <inheritdoc />
    public ASObject(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<ASObject, ASObjectEntity>(isExtending);

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TModel, TEntity}()" />
    public ASObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public ASObject(TypeMap typeMap, ASObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ASObject, ASObjectEntity>();

    static ASObject IASModel<ASObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ASObjectEntity Entity { get; }


    /// <summary>
    ///     Identifies a resource attached or related to an object that potentially requires special handling.
    ///     The intent is to provide a model that is at least semantically similar to attachments in email.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attachment" />
    public LinkableList<ASObject> Attachment
    {
        get => Entity.Attachment;
        set => Entity.Attachment = value;
    }

    /// <summary>
    ///     Identifies one or more entities that represent the total population of entities for which the object can considered to be relevant.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-audience" />
    public LinkableList<ASObject> Audience
    {
        get => Entity.Audience;
        set => Entity.Audience = value;
    }

    /// <summary>
    ///     Identifies one or more Objects that are part of the private secondary audience of this Object.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-bcc" />
    public LinkableList<ASObject> BCC
    {
        get => Entity.BCC;
        set => Entity.BCC = value;
    }

    /// <summary>
    ///     Identifies an Object that is part of the private primary audience of this Object.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-bto" />
    public LinkableList<ASObject> BTo
    {
        get => Entity.BTo;
        set => Entity.BTo = value;
    }

    /// <summary>
    ///     Identifies an Object that is part of the public secondary audience of this Object.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-cc" />
    public LinkableList<ASObject> CC
    {
        get => Entity.CC;
        set => Entity.CC = value;
    }

    /// <summary>
    ///     Identifies the context within which the object exists or an activity was performed.
    ///     THIS IS *NOT* THE JSON-LD CONTEXT! For that, you need <see cref="TypeMap.LDContext" />
    /// </summary>
    /// <remarks>
    ///     The notion of "context" used is intentionally vague.
    ///     The intended function is to serve as a means of grouping objects and activities that share a common originating context or purpose.
    ///     An example could be all activities relating to a common project or event.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/LikeActivity#dfn-context" />
    public Linkable<ASObject>? Context
    {
        get => Entity.Context;
        set => Entity.Context = value;
    }

    /// <summary>
    ///     Identifies the entity (e.g. an application) that generated the object.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-generator" />
    public Linkable<ASObject>? Generator
    {
        get => Entity.Generator;
        set => Entity.Generator = value;
    }

    /// <summary>
    ///     Indicates an entity that describes an icon for this object.
    ///     The image should have an aspect ratio of one (horizontal) to one (vertical) and should be suitable for presentation at a small size.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-icon" />
    public Linkable<ImageObject>? Icon
    {
        get => Entity.Icon;
        set => Entity.Icon = value;
    }

    /// <summary>
    ///     Indicates an entity that describes an image for this object.
    ///     Unlike the icon property, there are no aspect ratio or display size limitations assumed.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-image" />
    public Linkable<ImageObject>? Image
    {
        get => Entity.Image;
        set => Entity.Image = value;
    }

    /// <summary>
    ///     Indicates one or more entities for which this object is considered a response.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-inReplyTo" />
    public Linkable<ASObject>? InReplyTo
    {
        get => Entity.InReplyTo;
        set => Entity.InReplyTo = value;
    }

    /// <summary>
    ///     Indicates one or more physical or logical locations associated with the object.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-location" />
    public Linkable<ASObject>? Location
    {
        get => Entity.Location;
        set => Entity.Location = value;
    }

    /// <summary>
    ///     Identifies a Collection containing objects considered to be responses to this object.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-replies" />
    public ASCollection? Replies
    {
        get => Entity.Replies;
        set => Entity.Replies = value;
    }

    /// <summary>
    ///     One or more "tags" that have been associated with an objects.
    ///     A tag can be any kind of Object.
    ///     The key difference between attachment and tag is that the former implies association by inclusion, while the latter implies associated by reference.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-tag" />
    public LinkableList<ASObject> Tag
    {
        get => Entity.Tag;
        set => Entity.Tag = value;
    }

    /// <summary>
    ///     Identifies an entity considered to be part of the public primary audience of an Object
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-to" />
    public LinkableList<ASObject> To
    {
        get => Entity.To;
        set => Entity.To = value;
    }

    /// <summary>
    ///     Identifies one or more links to representations of the object
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-url" />
    public List<ASLink> Url
    {
        get => Entity.Url;
        set => Entity.Url = value;
    }

    /// <summary>
    ///     The content or textual representation of the Object encoded as a JSON string.
    ///     By default, the value of content is HTML.
    ///     The <see cref="ASType.MediaType" /> property can be used in the object to indicate a different content type.
    /// </summary>
    /// <remarks>
    ///     In JSON, this property may be mapped to either or both of "content" and "contentMap".
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-content" />
    public NaturalLanguageString? Content
    {
        get => Entity.ContentMap;
        set => Entity.ContentMap = value;
    }

    /// <summary>
    ///     When the object describes a time-bound resource, such as an audio or video, a meeting, etc, the duration property indicates the object's approximate duration.
    ///     The value MUST be expressed as an xsd:duration as defined by [xmlschema11-2], section 3.3.6 (e.g. a period of 5 seconds is represented as "PT5S").
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-duration" />
    /// <seealso href="https://www.w3.org/TR/xmlschema11-2/#duration" />
    public string? Duration
    {
        get => Entity.Duration;
        set => Entity.Duration = value;
    }

    /// <summary>
    ///     The date and time describing the actual or expected starting time of the object.
    ///     When used with an Activity object, for instance, the startTime property specifies the moment the activity began or is scheduled to begin.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-startTime" />
    public DateTime? StartTime
    {
        get => Entity.StartTime;
        set => Entity.StartTime = value;
    }

    /// <summary>
    ///     The date and time describing the actual or expected ending time of the object.
    ///     When used with an Activity object, for instance, the endTime property specifies the moment the activity concluded or is expected to conclude.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-endTime" />
    public DateTime? EndTime
    {
        get => Entity.EndTime;
        set => Entity.EndTime = value;
    }

    /// <summary>
    ///     The date and time at which the object was published.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-published" />
    public DateTime? Published
    {
        get => Entity.Published;
        set => Entity.Published = value;
    }

    /// <summary>
    ///     A natural language summarization of the object encoded as HTML.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-summary" />
    public NaturalLanguageString? Summary
    {
        get => Entity.Summary;
        set => Entity.Summary = value;
    }

    /// <summary>
    ///     The date and time at which the object was updated.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-updated" />
    public DateTime? Updated
    {
        get => Entity.Updated;
        set => Entity.Updated = value;
    }

    /// <summary>
    ///     The source property is intended to convey some sort of source from which the content markup was derived, as a form of provenance, or to support future editing by clients.
    ///     In general, clients do the conversion from source to content, not the other way around.
    /// </summary>
    /// <remarks>
    ///     This property is defined by ActivityPub, not ActivityStreams.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitypub/#source-property" />
    public ASObject? Source
    {
        get => Entity.Source;
        set => Entity.Source = value;
    }

    /// <summary>
    ///     This is a list of all Like activities with this object as the object property, added as a side effect.
    /// </summary>
    /// <remarks>
    ///     Care should be taken to not confuse the the likes collection with the similarly named but different <see cref="APActor.Liked" /> collection.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitypub/#likes" />
    public Linkable<ASCollection>? Likes
    {
        get => Entity.Likes;
        set => Entity.Likes = value;
    }

    /// <summary>
    ///     This is a list of all Announce activities with this object as the object property, added as a side effect.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#shares" />
    public Linkable<ASCollection>? Shares
    {
        get => Entity.Shares;
        set => Entity.Shares = value;
    }
}

/// <inheritdoc cref="ASObject" />
public sealed class ASObjectEntity : ASEntity<ASObject, ASObjectEntity>, IJsonOnDeserialized, IJsonOnSerializing
{
    /// <inheritdoc cref="ASObject.Attachment" />
    [JsonPropertyName("attachment")]
    public LinkableList<ASObject> Attachment { get; set; } = new();

    /// <inheritdoc cref="ASObject.Audience" />
    [JsonPropertyName("audience")]
    public LinkableList<ASObject> Audience { get; set; } = new();

    /// <inheritdoc cref="ASObject.BCC" />
    [JsonPropertyName("bcc")]
    public LinkableList<ASObject> BCC { get; set; } = new();

    /// <inheritdoc cref="ASObject.BTo" />
    [JsonPropertyName("bto")]
    public LinkableList<ASObject> BTo { get; set; } = new();

    /// <inheritdoc cref="ASObject.CC" />
    [JsonPropertyName("cc")]
    public LinkableList<ASObject> CC { get; set; } = new();

    /// <inheritdoc cref="ASObject.Context" />
    [JsonPropertyName("context")]
    public Linkable<ASObject>? Context { get; set; }

    /// <inheritdoc cref="ASObject.Generator" />
    [JsonPropertyName("generator")]
    public Linkable<ASObject>? Generator { get; set; }

    /// <inheritdoc cref="ASObject.Icon" />
    [JsonPropertyName("icon")]
    public Linkable<ImageObject>? Icon { get; set; }

    /// <inheritdoc cref="ASObject.Image" />
    [JsonPropertyName("image")]
    public Linkable<ImageObject>? Image { get; set; }

    /// <inheritdoc cref="ASObject.InReplyTo" />
    [JsonPropertyName("inReplyTo")]
    public Linkable<ASObject>? InReplyTo { get; set; }

    /// <inheritdoc cref="ASObject.Location" />
    [JsonPropertyName("location")]
    public Linkable<ASObject>? Location { get; set; }

    /// <inheritdoc cref="ASObject.Replies" />
    [JsonPropertyName("replies")]
    public ASCollection? Replies { get; set; }

    /// <inheritdoc cref="ASObject.Tag" />
    [JsonPropertyName("tag")]
    public LinkableList<ASObject> Tag { get; set; } = new();

    /// <inheritdoc cref="ASObject.To" />
    [JsonPropertyName("to")]
    public LinkableList<ASObject> To { get; set; } = new();

    /// <inheritdoc cref="ASObject.Url" />
    [JsonPropertyName("url")]
    public List<ASLink> Url { get; set; } = new();

    /// <inheritdoc cref="ASObject.Content" />
    [JsonPropertyName("contentMap")]
    public NaturalLanguageString? ContentMap { get; set; }

    /// <inheritdoc cref="ASObject.Content" />
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    /// <inheritdoc cref="ASObject.Duration" />
    [JsonPropertyName("duration")]
    public string? Duration { get; set; }

    /// <inheritdoc cref="ASObject.StartTime" />
    [JsonPropertyName("startTime")]
    public DateTime? StartTime { get; set; }

    /// <inheritdoc cref="ASObject.EndTime" />
    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; set; }

    /// <inheritdoc cref="ASObject.Published" />
    [JsonPropertyName("published")]
    public DateTime? Published { get; set; }

    /// <inheritdoc cref="ASObject.Summary" />
    [JsonPropertyName("summary")]
    public NaturalLanguageString? Summary { get; set; }

    /// <inheritdoc cref="ASObject.Updated" />
    [JsonPropertyName("updated")]
    public DateTime? Updated { get; set; }

    /// <inheritdoc cref="ASObject.Source" />
    [JsonPropertyName("source")]
    public ASObject? Source { get; set; }

    /// <inheritdoc cref="ASObject.Likes" />
    [JsonPropertyName("likes")]
    public Linkable<ASCollection>? Likes { get; set; }

    /// <inheritdoc cref="ASObject.Shares" />
    [JsonPropertyName("shares")]
    public Linkable<ASCollection>? Shares { get; set; }
    
    // Sync up "content" and "contentMap" properties
    void IJsonOnDeserialized.OnDeserialized()
    {
        if (Content == null)
            return;

        if (ContentMap == null)
            ContentMap = Content;
        else
            ContentMap.DefaultValue = Content;
    }
    
    // Sync up "content" and "contentMap" properties
    void IJsonOnSerializing.OnSerializing()
    {
        Content = ContentMap?.DefaultValue;

        if (ContentMap?.HasLanguages == false)
            ContentMap = null;
    }
}