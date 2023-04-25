/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using ActivityPub.Types.Util;

namespace ActivityPub.Types;

// TODO find a formal specification somewhere to verify whether these properties should be Link or Linkable<>

/// <summary>
/// Required properties for an Actor.
/// Specific Actor implementations should implement this.
/// </summary>
/// <remarks>
/// This is a synthetic type included for convenience
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitypub/#actor-objects"/>
public interface IActor
{
    /// <summary>
    /// A reference to an ActivityStreams OrderedCollection comprised of all the messages received by the actor.
    /// The inbox stream contains all activities received by the actor.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#inbox"/>
    public ASLink Inbox { get; set; }
    
    /// <summary>
    /// A reference to an ActivityStreams OrderedCollection comprised of all the messages produced by the actor.
    /// The outbox stream contains activities the user has published, subject to the ability of the requester to retrieve the activity.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#outbox"/>
    public ASLink Outbox { get; set; }
    
    /// <summary>
    /// A reference to an ActivityStreams collection of the actors that this actor is following.
    /// This is a list of everybody that the actor has followed, added as a side effect.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#following"/>
    public ASLink? Following { get; set; }
    
    /// <summary>
    /// A reference to an ActivityStreams collection of the actors that follow this actor.
    /// This is a list of everyone who has sent a Follow activity for the actor, added as a side effect.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#followers"/>
    public ASLink? Followers { get; set; }
    
    /// <summary>
    /// A reference to an ActivityStreams collection of objects this actor has liked.
    /// This is a list of every object from all of the actor's Like activities, added as a side effect.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#liked"/>
    public ASLink? Liked { get; set; }
    
    /// <summary>
    /// A list of supplementary Collections which may be of interest. 
    /// </summary>
    /// <remarks>
    /// Not sure what type this is.
    /// Maybe a link to a collection?
    /// </remarks>
    public ASType? Streams { get; set; }
    
    /// <summary>
    /// A short username which may be used to refer to the actor, with no uniqueness guarantees. 
    /// </summary>
    public NaturalLanguageString? PreferredUsername { get; set; }

    /// <summary>
    /// A json object which maps additional (typically server/domain-wide) endpoints which may be useful either for this actor or someone referencing this actor.
    /// This mapping may be nested inside the actor document as the value or may be a link to a JSON-LD document with these properties. 
    /// </summary>
    public Linkable<ActorEndpoints>? Endpoints { get; set; }
}

/// <summary>
/// An object that implements the required properties of an ActivityPub Actor.
/// </summary>
/// <remarks>
/// This base type is not required for implementing an actor.
/// Any ASObject can be used as long as it implements the required properties.
/// You can use <see cref="IActor"/> if you need to extend another base type.
///
/// This class can be constructed directly with a custom DefaultType to quickly create a custom Actor.
///
/// This is a synthetic class included for utility.
/// It does not exist in the ActivityStreams or ActivityPub standards.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitypub/#actor-objects"/>
public class ASActor : ASObject, IActor
{
    public ASActor(string type) : base(type) {}

    public required ASLink Inbox { get; set; } 
    public required ASLink Outbox { get; set; }
    public ASLink? Following { get; set; }
    public ASLink? Followers { get; set; }
    public ASLink? Liked { get; set; }
    public ASType? Streams { get; set; }
    public NaturalLanguageString? PreferredUsername { get; set; }
    public Linkable<ActorEndpoints>? Endpoints { get; set; }
}

/// <summary>
/// A json object which maps additional (typically server/domain-wide) endpoints which may be useful for an actor.
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitypub/#actor-objects"/>
public class ActorEndpoints : ASObject
{
    /// <summary>
    /// Endpoint URI so this actor's clients may access remote ActivityStreams objects which require authentication to access.
    /// To use this endpoint, the client posts an x-www-form-urlencoded id parameter with the value being the id of the requested ActivityStreams object. 
    /// </summary>
    public ASLink? ProxyUrl { get; set; }
    
    /// <summary>
    /// If OAuth 2.0 bearer tokens [RFC6749] [RFC6750] are being used for authenticating client to server interactions, this endpoint specifies a URI at which a browser-authenticated user may obtain a new authorization grant.
    /// </summary>
    public ASLink? OAuthAuthorizationEndpoint { get; set; }
    
    /// <summary>
    /// If OAuth 2.0 bearer tokens [RFC6749] [RFC6750] are being used for authenticating client to server interactions, this endpoint specifies a URI at which a client may acquire an access token. 
    /// </summary>
    public ASLink? OAuthTokenEndpoint { get; set; }
    
    /// <summary>
    /// If Linked Data Signatures and HTTP Signatures are being used for authentication and authorization, this endpoint specifies a URI at which browser-authenticated users may authorize a client's public key for client to server interactions. 
    /// </summary>
    public ASLink? ProvideClientKey { get; set; }
    
    /// <summary>
    /// If Linked Data Signatures and HTTP Signatures are being used for authentication and authorization, this endpoint specifies a URI at which a client key may be signed by the actor's key for a time window to act on behalf of the actor in interacting with foreign servers. 
    /// </summary>
    public ASLink? SignClientKey { get; set; }
    
    /// <summary>
    /// An optional endpoint used for wide delivery of publicly addressed activities and activities sent to followers.
    /// SharedInbox endpoints SHOULD also be publicly readable OrderedCollection objects containing objects addressed to the Public special collection.
    /// Reading from the sharedInbox endpoint MUST NOT present objects which are not addressed to the Public endpoint. 
    /// </summary>
    public ASLink? SharedInbox { get; set; }
}