// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Collection;
using ActivityPub.Types.Extended.Activity;
using ActivityPub.Types.Extended.Object;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// Describes an object of any kind.
/// The Object type serves as the base type for most of the other kinds of objects defined in the Activity Vocabulary, including other Core types such as Activity, IntransitiveActivity, Collection and OrderedCollection. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object" />
[ASTypeKey(ObjectType)]
public class ASObject : ASType
{
    public const string ObjectType = "Object";

    [JsonConstructor]
    public ASObject() : this(ObjectType) {}

    public ASObject(string type) : base(type) {}

    /// <summary>
    /// Identifies a resource attached or related to an object that potentially requires special handling.
    /// The intent is to provide a model that is at least semantically similar to attachments in email. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attachment"/>
    public LinkableList<ASObject> Attachment { get; set; } = new();

    /// <summary>
    /// Identifies one or more entities that represent the total population of entities for which the object can considered to be relevant.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-audience"/>
    public LinkableList<ASObject> Audience { get; set; } = new();

    /// <summary>
    /// Identifies one or more Objects that are part of the private secondary audience of this Object. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-bcc"/>
    public LinkableList<ASObject> BCC { get; set; } = new();

    /// <summary>
    /// Identifies an Object that is part of the private primary audience of this Object.  
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-bto"/>
    [JsonPropertyName("bto")] // this property is not in camelcase for some reason
    public LinkableList<ASObject> BTo { get; set; } = new();

    /// <summary>
    /// Identifies an Object that is part of the public secondary audience of this Object.   
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-cc"/>
    public LinkableList<ASObject> CC { get; set; } = new();

    /// <summary>
    /// Identifies the context within which the object exists or an activity was performed.
    /// THIS IS *NOT* THE JSON-LD CONTEXT! For that, you need <see cref="ASType.JsonLdContexts"/>
    /// </summary>
    /// <remarks>
    /// The notion of "context" used is intentionally vague.
    /// The intended function is to serve as a means of grouping objects and activities that share a common originating context or purpose.
    /// An example could be all activities relating to a common project or event.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-context"/>
    public Linkable<ASObject>? Context { get; set; }

    /// <summary>
    /// Identifies the entity (e.g. an application) that generated the object. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-generator"/>
    public Linkable<ASObject>? Generator { get; set; }

    /// <summary>
    /// Indicates an entity that describes an icon for this object.
    /// The image should have an aspect ratio of one (horizontal) to one (vertical) and should be suitable for presentation at a small size. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-icon"/>
    public Linkable<ImageObject>? Icon { get; set; }

    /// <summary>
    /// Indicates an entity that describes an image for this object.
    /// Unlike the icon property, there are no aspect ratio or display size limitations assumed. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-image"/>
    public Linkable<ImageObject>? Image { get; set; }

    /// <summary>
    /// Indicates one or more entities for which this object is considered a response. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-inReplyTo"/>
    public Linkable<ASObject>? InReplyTo { get; set; }

    /// <summary>
    /// Indicates one or more physical or logical locations associated with the object. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-location"/>
    public Linkable<ASObject>? Location { get; set; }

    /// <summary>
    /// Identifies a Collection containing objects considered to be responses to this object. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-replies"/>
    public ASCollection<ASObject>? Replies { get; set; }

    /// <summary>
    /// One or more "tags" that have been associated with an objects.
    /// A tag can be any kind of Object.
    /// The key difference between attachment and tag is that the former implies association by inclusion, while the latter implies associated by reference. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-tag"/>
    public LinkableList<ASObject> Tag { get; set; } = new();

    /// <summary>
    /// Identifies an entity considered to be part of the public primary audience of an Object 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-to"/>
    public LinkableList<ASObject> To { get; set; } = new();

    /// <summary>
    /// Identifies one or more links to representations of the object 
    /// </summary>
    /// <remarks>
    /// TODO this needs a special case - it can be a List{ASLink}, ASLink, *or* ASUri
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-url"/>
    public ASLink? Url { get; set; }

    /// <summary>
    /// The content or textual representation of the Object encoded as a JSON string.
    /// By default, the value of content is HTML.
    /// The mediaType property can be used in the object to indicate a different content type.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-content"/>
    public NaturalLanguageString? Content { get; set; }

    /// <summary>
    /// When the object describes a time-bound resource, such as an audio or video, a meeting, etc, the duration property indicates the object's approximate duration.
    /// The value MUST be expressed as an xsd:duration as defined by [xmlschema11-2], section 3.3.6 (e.g. a period of 5 seconds is represented as "PT5S").
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-duration"/>
    /// <seealso href="https://www.w3.org/TR/xmlschema11-2/#duration"/>
    public string? Duration { get; set; }

    /// <summary>
    /// The date and time describing the actual or expected starting time of the object.
    /// When used with an Activity object, for instance, the startTime property specifies the moment the activity began or is scheduled to begin. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-startTime"/>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// The date and time describing the actual or expected ending time of the object.
    /// When used with an Activity object, for instance, the endTime property specifies the moment the activity concluded or is expected to conclude. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-endTime"/>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// The date and time at which the object was published.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-published"/>
    public DateTime? Published { get; set; }

    /// <summary>
    /// A natural language summarization of the object encoded as HTML.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-summary"/>
    public NaturalLanguageString? Summary { get; set; }

    /// <summary>
    /// The date and time at which the object was updated.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-updated"/>
    public DateTime? Updated { get; set; }

    /// <summary>
    /// The source property is intended to convey some sort of source from which the content markup was derived, as a form of provenance, or to support future editing by clients.
    /// In general, clients do the conversion from source to content, not the other way around. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#source-property"/>
    public ASObject? Source { get; set; }

    /// <summary>
    /// This is a list of all Like activities with this object as the object property, added as a side effect.
    /// </summary>
    /// <remarks>
    /// Care should be taken to not confuse the the likes collection with the similarly named but different <see cref="IActor.Liked"/> collection.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitypub/#likes"/>
    public Linkable<ASCollection<LikeActivity>>? Likes { get; set; }

    /// <summary>
    /// This is a list of all Announce activities with this object as the object property, added as a side effect.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#shares"/>
    public Linkable<ASCollection<AnnounceActivity>>? Shares { get; set; }
}