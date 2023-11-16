// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.AS;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Util.Fakes;

public class EmptyExtendedTypeFake : ASType, IASModel<EmptyExtendedTypeFake, EmptyExtendedTypeFakeEntity, ASType>
{
    public static JsonLDContextObject ExtendedContext { get; } = "https://example.com/context";
    public static IJsonLDContext DefiningContext { get; } = new JsonLDContext()
    {
        JsonLDContextObject.ActivityStreams,
        ExtendedContext
    };

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public EmptyExtendedTypeFake() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public EmptyExtendedTypeFake(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<EmptyExtendedTypeFakeEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public EmptyExtendedTypeFake(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public EmptyExtendedTypeFake(TypeMap typeMap, EmptyExtendedTypeFakeEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<EmptyExtendedTypeFakeEntity>();

    static EmptyExtendedTypeFake IASModel<EmptyExtendedTypeFake>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private EmptyExtendedTypeFakeEntity Entity { get; }
}

public sealed class EmptyExtendedTypeFakeEntity : ASEntity<EmptyExtendedTypeFake, EmptyExtendedTypeFakeEntity> {}