// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a short written work typically less than a single paragraph in length.
/// </summary>
public class NoteObject : ASObject
{
    public NoteObject() => Entity = new NoteObjectEntity { TypeMap = TypeMap };
    public NoteObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<NoteObjectEntity>();
    private NoteObjectEntity Entity { get; }
}

/// <inheritdoc cref="NoteObject" />
[ASTypeKey(NoteType)]
[ImpliesOtherEntity(typeof(ASObjectEntity))]
public sealed class NoteObjectEntity : ASBase<NoteObject>
{
    public const string NoteType = "Note";
    public override string ASTypeName => NoteType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASObjectEntity.ObjectType
    };
}