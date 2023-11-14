// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a short written work typically less than a single paragraph in length.
/// </summary>
public class NoteObject : ASObject, IASModel<NoteObject, NoteObjectEntity, ASObject>
{
    public const string NoteType = "Note";
    static string IASModel<NoteObject>.ASTypeName => NoteType;

    public NoteObject() : this(new TypeMap()) {}

    public NoteObject(TypeMap typeMap) : base(typeMap)
    {
        Entity = new NoteObjectEntity();
        TypeMap.Add(Entity);
    }

    [SetsRequiredMembers]
    public NoteObject(TypeMap typeMap, NoteObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<NoteObjectEntity>();

    static NoteObject IASModel<NoteObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private NoteObjectEntity Entity { get; }
}

/// <inheritdoc cref="NoteObject" />
public sealed class NoteObjectEntity : ASEntity<NoteObject, NoteObjectEntity> {}