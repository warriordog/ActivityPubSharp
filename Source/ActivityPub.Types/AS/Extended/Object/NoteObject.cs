// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a short written work typically less than a single paragraph in length.
/// </summary>
public class NoteObject : ASObject, IASModel<NoteObject, NoteObjectEntity, ASObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Note" types.
    /// </summary>
    public const string NoteType = "Note";
    static string IASModel<NoteObject>.ASTypeName => NoteType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public NoteObject() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public NoteObject(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<NoteObjectEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public NoteObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public NoteObject(TypeMap typeMap, NoteObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<NoteObjectEntity>();

    static NoteObject IASModel<NoteObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private NoteObjectEntity Entity { get; }
}

/// <inheritdoc cref="NoteObject" />
public sealed class NoteObjectEntity : ASEntity<NoteObject, NoteObjectEntity> {}