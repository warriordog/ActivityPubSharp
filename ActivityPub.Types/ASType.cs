/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// Base type of all ActivityStreams / ActivityPub objects.
/// </summary>
/// <remarks>
/// This is a synthetic type created to help adapt ActivityStreams to the .NET object model.
/// It does not exist in the ActivityStreams standard.
/// </remarks>
public abstract class ASType
{
    protected ASType(string defaultType) => Types.Add(defaultType);

    /// <summary>
    /// Identifies the Object or Link types.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-type"/>
    public HashSet<string> Types { get; set; } = new();

    /// <summary>
    /// Lists the JSON-LD contexts used by this object.
    /// Should be a full URL
    /// </summary>
    public HashSet<string> Contexts { get; set; } = new();

    /// <summary>
    /// Provides the globally unique identifier for an Object or Link.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-id"/>
    public string? Id { get; set; }

    /// <summary>
    /// True if this object is anonymous and should be considered part of its parent context.
    /// </summary>
    /// <remarks>
    /// Based on https://www.w3.org/TR/activitypub/#obj-id
    /// </remarks>
    public bool IsAnonymous => Id == null;

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
}