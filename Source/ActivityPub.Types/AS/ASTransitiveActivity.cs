// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.AS;

/// <summary>
///     Base type for all activities that are not intransitive
/// </summary>
/// <remarks>
///     This is a synthetic type, and not part of the ActivityStreams standard.
/// </remarks>
public class ASTransitiveActivity : ASActivity, IASModel<ASTransitiveActivity, ASTransitiveActivityEntity, ASActivity>
{
    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public ASTransitiveActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ASTransitiveActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<ASTransitiveActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ASTransitiveActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public ASTransitiveActivity(TypeMap typeMap, ASTransitiveActivityEntity? entity) : base(typeMap, null)
    {
        Entity = entity ?? typeMap.AsEntity<ASTransitiveActivityEntity>();
        Object = Entity.Object;
    }

    static ASTransitiveActivity IASModel<ASTransitiveActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ASTransitiveActivityEntity Entity { get; }


    /// <summary>
    ///     Describes the direct object of the activity.
    ///     For instance, in the activity "John added a movie to his wishlist", the object of the activity is the movie added.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object" />
    [JsonPropertyName("object")]
    public required LinkableList<ASObject> Object
    {
        get => Entity.Object;
        set => Entity.Object = value;
    }
}

/// <inheritdoc cref="ASTransitiveActivity" />
public sealed class ASTransitiveActivityEntity : ASEntity<ASTransitiveActivity, ASTransitiveActivityEntity>, ISubTypeDeserialized
{
    /// <inheritdoc cref="ASTransitiveActivity.Object" />
    [JsonPropertyName("object")]
    public LinkableList<ASObject> Object { get; set; } = new();

    /// <inheritdoc />
    public static bool TryNarrowTypeByJson(JsonElement element, DeserializationMetadata meta, [NotNullWhen(true)] out Type? type)
    {
        // Only change type if its targeted (it has the "target" property)
        if (element.HasProperty("target"))
        {
            type = typeof(ASTargetedActivityEntity);
            return true;
        }

        type = null;
        return false;
    }
}