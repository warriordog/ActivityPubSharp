// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ActivityPub.Types.AS;

/// <summary>
///     A json object which maps additional (typically server/domain-wide) endpoints which may be useful for an actor.
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitypub/#actor-objects" />
public class APActorEndpoints : ASType, IASModel<APActorEndpoints, APActorEndpointsEntity, ASType>
{
    /// <inheritdoc />
    public APActorEndpoints() => Entity = TypeMap.Extend<APActorEndpoints, APActorEndpointsEntity>();

    /// <inheritdoc />
    public APActorEndpoints(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<APActorEndpoints, APActorEndpointsEntity>(isExtending);

    /// <inheritdoc />
    public APActorEndpoints(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public APActorEndpoints(TypeMap typeMap, APActorEndpointsEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<APActorEndpoints, APActorEndpointsEntity>();

    static APActorEndpoints IASModel<APActorEndpoints>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private APActorEndpointsEntity Entity { get; }
    
    /// <summary>
    ///     Endpoint URI so this actor's clients may access remote ActivityStreams objects which require authentication to access.
    ///     To use this endpoint, the client posts an x-www-form-urlencoded id parameter with the value being the id of the requested ActivityStreams object.
    /// </summary>
    [JsonPropertyName("proxyUrl")]
    public ASLink? ProxyUrl
    {
        get => Entity.ProxyUrl;
        set => Entity.ProxyUrl = value;
    }

    /// <summary>
    ///     If OAuth 2.0 bearer tokens [RFC6749] [RFC6750] are being used for authenticating client to server interactions, this endpoint specifies a URI at which a browser-authenticated user may obtain a new authorization grant.
    /// </summary>
    [JsonPropertyName("oauthAuthorizationEndpoint")]
    public ASLink? OAuthAuthorizationEndpoint
    {
        get => Entity.OAuthAuthorizationEndpoint;
        set => Entity.OAuthAuthorizationEndpoint = value;
    }

    /// <summary>
    ///     If OAuth 2.0 bearer tokens [RFC6749] [RFC6750] are being used for authenticating client to server interactions, this endpoint specifies a URI at which a client may acquire an access token.
    /// </summary>
    [JsonPropertyName("oauthTokenEndpoint")]
    public ASLink? OAuthTokenEndpoint
    {
        get => Entity.OAuthTokenEndpoint;
        set => Entity.OAuthTokenEndpoint = value;
    }

    /// <summary>
    ///     If Linked Data Signatures and HTTP Signatures are being used for authentication and authorization, this endpoint specifies a URI at which browser-authenticated users may authorize a client's public key for client to server interactions.
    /// </summary>
    [JsonPropertyName("provideClientKey")]
    public ASLink? ProvideClientKey
    {
        get => Entity.ProvideClientKey;
        set => Entity.ProvideClientKey = value;
    }

    /// <summary>
    ///     If Linked Data Signatures and HTTP Signatures are being used for authentication and authorization, this endpoint specifies a URI at which a client key may be signed by the actor's key for a time window to act on behalf of the actor in interacting with foreign servers.
    /// </summary>
    [JsonPropertyName("signClientKey")]
    public ASLink? SignClientKey
    {
        get => Entity.SignClientKey;
        set => Entity.SignClientKey = value;
    }

    /// <summary>
    ///     An optional endpoint used for wide delivery of publicly addressed activities and activities sent to followers.
    ///     SharedInbox endpoints SHOULD also be publicly readable OrderedCollection objects containing objects addressed to the Public special collection.
    ///     Reading from the sharedInbox endpoint MUST NOT present objects which are not addressed to the Public endpoint.
    /// </summary>
    [JsonPropertyName("sharedInbox")]
    public ASLink? SharedInbox
    {
        get => Entity.SharedInbox;
        set => Entity.SharedInbox = value;
    }
}

/// <inheritdoc cref="APActorEndpoints"/>
public sealed class APActorEndpointsEntity : ASEntity<APActorEndpoints, APActorEndpointsEntity>
{
    /// <inheritdoc cref="APActorEndpoints"/>
    [JsonPropertyName("proxyUrl")]
    public ASLink? ProxyUrl { get; set; }

    /// <inheritdoc cref="APActorEndpoints"/>
    [JsonPropertyName("oauthAuthorizationEndpoint")]
    public ASLink? OAuthAuthorizationEndpoint { get; set; }

    /// <inheritdoc cref="APActorEndpoints"/>
    [JsonPropertyName("oauthTokenEndpoint")]
    public ASLink? OAuthTokenEndpoint { get; set; }

    /// <inheritdoc cref="APActorEndpoints"/>
    [JsonPropertyName("provideClientKey")]
    public ASLink? ProvideClientKey { get; set; }

    /// <inheritdoc cref="APActorEndpoints"/>
    [JsonPropertyName("signClientKey")]
    public ASLink? SignClientKey { get; set; }

    /// <inheritdoc cref="APActorEndpoints"/>
    [JsonPropertyName("sharedInbox")]
    public ASLink? SharedInbox { get; set; }
}