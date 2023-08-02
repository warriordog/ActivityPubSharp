// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types;

// TODO placeholder! consider removing it.

// All types derive from this, INCLUDING ASType.
// It has NO properties, only the linking metadata
public abstract class ASBase
{
    /// <summary>
    /// Name of the ActivityStreams type that this object implements.
    /// Will be null for synthetic types.
    /// For the full list of names in the object graph, use <see cref="TypeMap.ASTypes"/>.
    /// </summary>
    public string? ASTypeName { get; }

    /// <summary>
    /// Creates an entity and attaches it to a specified type graph.
    /// </summary>
    /// <throws cref="InvalidOperationException">If an entity of this type already exists in the graph</throws>
    protected ASBase(string? asTypeName, TypeMap typeMap) : this(asTypeName)
        => typeMap.Add(this);

    /// <summary>
    /// Creates a free-floating entity, not attached to any graph.
    /// Unless you're writing JSON logic or unit tests, you probably want the other constructor.
    /// </summary>
    protected ASBase(string? asTypeName)
        => ASTypeName = asTypeName;
}