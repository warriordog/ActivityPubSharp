// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types;

// TODO placeholder! consider removing it.

// All types derive from this, INCLUDING ASType.
// It has NO properties, only the linking metadata
public abstract class ASBase
{
    /// <summary>
    /// Graph of all ActivityStreams types in the current object.
    /// Can be used to safely navigate from one type to any other.
    /// </summary>
    public TypeMap TypeMap { get; }

    /// <summary>
    /// Name of the ActivityStreams type that this object implements.
    /// Will be null for synthetic types.
    /// For the full list of names in the object graph, use <see cref="TypeMap.ASTypes"/>.
    /// </summary>
    public string? ASTypeName { get; }


    /// <summary>
    /// Creates an object and attaches it to a specified type graph.
    /// </summary>
    /// <throws cref="InvalidOperationException">If an object of this type already exists in the graph</throws>
    protected ASBase(string? asTypeName, TypeMap typeMap)
    {
        ASTypeName = asTypeName;
        
        TypeMap = typeMap;
        TypeMap.Add(this);
    }

    /// <summary>
    /// Creates an object and attaches it to the type graph of a target object.
    /// </summary>
    /// <throws cref="InvalidOperationException">If an object of this type already exists in the graph</throws>
    protected ASBase(string? asTypeName, ASBase baseType)
        : this(asTypeName, baseType.TypeMap)
    {}
    
    /// <summary>
    /// Creates an object and attaches it to a NEW type graph.
    /// </summary>
    protected ASBase(string? asTypeName)
        : this(asTypeName, new TypeMap())
    {}
    
    
    // /// <inheritdoc cref="TypeMap.Is{T}()"/>
    // public bool Is<T>() where T : ASBase
    //     => TypeMap.Is<T>();
    //
    // /// <inheritdoc cref="TypeMap.Is{T}(out T?)"/>
    // public bool Is<T>([NotNullWhen(true)] out T? instance) where T : ASBase
    //     => TypeMap.Is(out instance);
    //
    // /// <inheritdoc cref="TypeMap.As{T}"/>
    // public T As<T>() where T : ASBase
    //     => TypeMap.As<T>();
}