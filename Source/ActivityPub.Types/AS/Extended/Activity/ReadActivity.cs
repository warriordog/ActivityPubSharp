// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has read the object.
/// </summary>
public class ReadActivity : ASTransitiveActivity, IASModel<ReadActivity, ReadActivityEntity, ASTransitiveActivity>
{
    public const string ReadType = "Read";
    static string IASModel<ReadActivity>.ASTypeName => ReadType;

    public ReadActivity() : this(new TypeMap()) {}

    public ReadActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new ReadActivityEntity();
        TypeMap.Add(Entity);
    }

    [SetsRequiredMembers]
    public ReadActivity(TypeMap typeMap, ReadActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ReadActivityEntity>();

    static ReadActivity IASModel<ReadActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ReadActivityEntity Entity { get; }
}

/// <inheritdoc cref="ReadActivity" />
public sealed class ReadActivityEntity : ASEntity<ReadActivity, ReadActivityEntity> {}