// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.AS;
using ActivityPub.Types.Internal;

namespace ActivityPub.Types.Tests.Util.Fakes;

public class AnonymousExtensionFake : ASObject, IASModel<AnonymousExtensionFake, AnonymousExtensionFakeEntity, ASObject>
{
    /// <inheritdoc />
    public AnonymousExtensionFake(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<AnonymousExtensionFake, AnonymousExtensionFakeEntity>(isExtending);

    /// <inheritdoc />
    public AnonymousExtensionFake(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public AnonymousExtensionFake(TypeMap typeMap, AnonymousExtensionFakeEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<AnonymousExtensionFake, AnonymousExtensionFakeEntity>();

    static AnonymousExtensionFake IASModel<AnonymousExtensionFake>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private AnonymousExtensionFakeEntity Entity { get; }

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

    public static bool? ShouldConvertFrom(JsonElement inputJson, TypeMap typeMap) =>
        inputJson.HasProperty(nameof(ExtendedString)) 
        || inputJson.HasProperty(nameof(ExtendedInt));
}

public sealed class AnonymousExtensionFakeEntity : ASEntity<AnonymousExtensionFake, AnonymousExtensionFakeEntity>
{
    public string ExtendedString { get; set; } = "";
    public int ExtendedInt { get; set; }
}