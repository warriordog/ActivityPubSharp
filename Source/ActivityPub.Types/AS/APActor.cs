// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.AS;

/// <summary>
///     An object that implements the required properties of an ActivityPub Actor.
/// </summary>
/// <remarks>
///     This is a synthetic class included for utility.
///     It does not exist in the ActivityStreams or ActivityPub standards.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitypub/#actor-objects" />
public class APActor : ASObject, IASModel<APActor, APActorEntity, ASObject>
{
    /// <inheritdoc />
    public APActor() => Entity = TypeMap.Extend<APActor, APActorEntity>();

    /// <inheritdoc />
    public APActor(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<APActor, APActorEntity>(isExtending);

    /// <inheritdoc />
    public APActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public APActor(TypeMap typeMap, APActorEntity? entity) : base(typeMap, null)
    {
        Entity = entity ?? typeMap.AsEntity<APActor, APActorEntity>();
        Inbox = Entity.Inbox ?? throw new ArgumentException($"The provided entity is invalid - required {nameof(APActorEntity.Inbox)} property is missing");
        Outbox = Entity.Outbox ?? throw new ArgumentException($"The provided entity is invalid - required {nameof(APActorEntity.Outbox)} property is missing");
    }

    static APActor IASModel<APActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private APActorEntity Entity { get; }


    /// <summary>
    ///     A reference to an ActivityStreams OrderedCollection comprised of all the messages received by the actor.
    ///     The inbox stream contains all activities received by the actor.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#inbox" />
    public required ASLink Inbox
    {
        get => Entity.Inbox!;
        set => Entity.Inbox = value;
    }

    /// <summary>
    ///     A reference to an ActivityStreams OrderedCollection comprised of all the messages produced by the actor.
    ///     The outbox stream contains activities the user has published, subject to the ability of the requester to retrieve the activity.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#outbox" />
    public required ASLink Outbox
    {
        get => Entity.Outbox!;
        set => Entity.Outbox = value;
    }

    /// <summary>
    ///     A reference to an ActivityStreams collection of the actors that this actor is following.
    ///     This is a list of everybody that the actor has followed, added as a side effect.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#following" />
    public ASLink? Following
    {
        get => Entity.Following;
        set => Entity.Following = value;
    }

    /// <summary>
    ///     A reference to an ActivityStreams collection of the actors that follow this actor.
    ///     This is a list of everyone who has sent a Follow activity for the actor, added as a side effect.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#followers" />
    public ASLink? Followers
    {
        get => Entity.Followers;
        set => Entity.Followers = value;
    }

    /// <summary>
    ///     A reference to an ActivityStreams collection of objects this actor has liked.
    ///     This is a list of every object from all of the actor's Like activities, added as a side effect.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitypub/#liked" />
    public ASLink? Liked
    {
        get => Entity.Liked;
        set => Entity.Liked = value;
    }

    /// <summary>
    ///     A list of supplementary Collections which may be of interest.
    /// </summary>
    /// <remarks>
    ///     Not sure what type this is.
    ///     Maybe a link to a collection?
    /// </remarks>
    public ASType? Streams
    {
        get => Entity.Streams;
        set => Entity.Streams = value;
    }

    /// <summary>
    ///     A short username which may be used to refer to the actor, with no uniqueness guarantees.
    /// </summary>
    public NaturalLanguageString? PreferredUsername
    {
        get => Entity.PreferredUsername;
        set => Entity.PreferredUsername = value;
    }

    /// <summary>
    ///     A json object which maps additional (typically server/domain-wide) endpoints which may be useful either for this actor or someone referencing this actor.
    ///     This mapping may be nested inside the actor document as the value or may be a link to a JSON-LD document with these properties.
    /// </summary>
    /// <remarks>
    ///     This should technically be a Linkable{ActorEndpoints}, but ActorEndpoints does not extend ASType
    /// </remarks>
    public Linkable<APActorEndpoints>? Endpoints
    {
        get => Entity.Endpoints;
        set => Entity.Endpoints = value;
    }
    
    /// <inheritdoc />
    static bool? IASModel<APActor>.ShouldConvertFrom(JsonElement inputJson, TypeMap typeMap)
    {
        if (inputJson.ValueKind != JsonValueKind.Object)
            return false;

        return
            inputJson.HasProperty("inbox") &&
            inputJson.HasProperty("outbox");
    }
}

/// <inheritdoc cref="APActor" />
public sealed class APActorEntity : ASEntity<APActor, APActorEntity>
{
    /// <inheritdoc cref="APActor.Inbox" />
    [JsonPropertyName("inbox")]
    public ASLink? Inbox { get; set; }

    /// <inheritdoc cref="APActor.Outbox" />
    [JsonPropertyName("outbox")]
    public ASLink? Outbox { get; set; }

    /// <inheritdoc cref="APActor.Following" />
    [JsonPropertyName("following")]
    public ASLink? Following { get; set; }

    /// <inheritdoc cref="APActor.Followers" />
    [JsonPropertyName("followers")]
    public ASLink? Followers { get; set; }

    /// <inheritdoc cref="APActor.Liked" />
    [JsonPropertyName("liked")]
    public ASLink? Liked { get; set; }

    /// <inheritdoc cref="APActor.Streams" />
    [JsonPropertyName("streams")]
    public ASType? Streams { get; set; }

    /// <inheritdoc cref="APActor.PreferredUsername" />
    [JsonPropertyName("preferredUsername")]
    public NaturalLanguageString? PreferredUsername { get; set; }

    /// <inheritdoc cref="APActor.Endpoints" />
    [JsonPropertyName("endpoints")]
    public Linkable<APActorEndpoints>? Endpoints { get; set; }
}