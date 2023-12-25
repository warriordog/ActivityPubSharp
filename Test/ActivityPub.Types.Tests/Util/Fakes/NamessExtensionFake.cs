// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.AS;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Util.Fakes;

public class NamelessExtensionFake : ASObject, IASModel<NamelessExtensionFake, NamelessExtensionFakeEntity, ASObject>
{
    static IJsonLDContext IASModel<NamelessExtensionFake>.DefiningContext { get; } = new JsonLDContext
    {
        JsonLDContextObject.ActivityStreams,
        "https://example.com/nameless"
    };

    /// <inheritdoc />
    public NamelessExtensionFake() => Entity = TypeMap.Extend<NamelessExtensionFake, NamelessExtensionFakeEntity>();

    /// <inheritdoc />
    public NamelessExtensionFake(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<NamelessExtensionFake, NamelessExtensionFakeEntity>(isExtending);

    /// <inheritdoc />
    public NamelessExtensionFake(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public NamelessExtensionFake(TypeMap typeMap, NamelessExtensionFakeEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<NamelessExtensionFake, NamelessExtensionFakeEntity>();

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

public sealed class NamelessExtensionFakeEntity : ASEntity<NamelessExtensionFake, NamelessExtensionFakeEntity>
{
    public string ExtendedString { get; set; } = "";
    public int ExtendedInt { get; set; }
}