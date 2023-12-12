// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Util.Fakes;

public class NamelessExtensionFake : ASObject, IASModel<NamelessExtensionFake, NamelessExtensionFakeEntity, ASObject>
{
    public static JsonLDContextObject NamelessExtensionContext { get;  }= "https://example.com/nameless";
    static IJsonLDContext IASModel<NamelessExtensionFake>.DefiningContext { get; } = new JsonLDContext
    {
        JsonLDContextObject.ActivityStreams,
        NamelessExtensionContext
    };

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public NamelessExtensionFake() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public NamelessExtensionFake(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<NamelessExtensionFakeEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public NamelessExtensionFake(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public NamelessExtensionFake(TypeMap typeMap, NamelessExtensionFakeEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<NamelessExtensionFakeEntity>();

    static NamelessExtensionFake IASModel<NamelessExtensionFake>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private NamelessExtensionFakeEntity Entity { get; }

    public string ExtendedString
    {
        get => Entity.ExtendedString;
        set => Entity.ExtendedString = value;
    }

    public int ExtendedInt
    {
        get => Entity.ExtendedInt;
        set => Entity.ExtendedInt = value;
    }
}

public sealed class NamelessExtensionFakeEntity : ASEntity<NamelessExtensionFake, NamelessExtensionFakeEntity>, INamelessEntity
{
    public string ExtendedString { get; set; } = "";
    public int ExtendedInt { get; set; }

    public static bool ShouldConvertFrom(IJsonLDContext jsonLDContext, DeserializationMetadata meta)
        => jsonLDContext.Contains(NamelessExtensionFake.NamelessExtensionContext);
}