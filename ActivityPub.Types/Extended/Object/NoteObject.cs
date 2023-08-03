// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a short written work typically less than a single paragraph in length. 
/// </summary>
public class NoteObject : ASObject
{
    private NoteObjectEntity Entity { get; }

    public NoteObject() => Entity = new NoteObjectEntity(TypeMap);
    public NoteObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<NoteObjectEntity>();
}

/// <inheritdoc cref="NoteObject"/>
[ASTypeKey(NoteType)]
[ImpliesOtherEntity(typeof(ASObjectEntity))]
public sealed class NoteObjectEntity : ASBase
{
    public const string NoteType = "Note";

    /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public NoteObjectEntity(TypeMap typeMap) : base(NoteType, typeMap) {}

    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public NoteObjectEntity() : base(NoteType) {}
}