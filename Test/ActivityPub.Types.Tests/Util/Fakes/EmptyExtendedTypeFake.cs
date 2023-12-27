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

    /// <inheritdoc />
    public EmptyExtendedTypeFake() => Entity = TypeMap.Extend<EmptyExtendedTypeFake, EmptyExtendedTypeFakeEntity>();

    /// <inheritdoc />
    public EmptyExtendedTypeFake(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<EmptyExtendedTypeFake, EmptyExtendedTypeFakeEntity>(isExtending);

    /// <inheritdoc />
    public EmptyExtendedTypeFake(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public EmptyExtendedTypeFake(TypeMap typeMap, EmptyExtendedTypeFakeEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<EmptyExtendedTypeFake, EmptyExtendedTypeFakeEntity>();

    static EmptyExtendedTypeFake IASModel<EmptyExtendedTypeFake>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private EmptyExtendedTypeFakeEntity Entity { get; }
}

public sealed class EmptyExtendedTypeFakeEntity : ASEntity<EmptyExtendedTypeFake, EmptyExtendedTypeFakeEntity> {}