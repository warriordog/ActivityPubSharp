// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.Util;

namespace ActivityPub.Types;

// TODO model here is WIP

// public interface IAPType
// {
//     static abstract string APTypeName { get; }
// }

// public interface IAPInstance
// {
//     /// <summary>
//     /// Graph of all ActivityStreams types in the current object.
//     /// Can be used to safely navigate from one type to any other.
//     /// </summary>
//     public TypeMap TypeMap { get; }
//     
//     /// <inheritdoc cref="TypeMap.Is{T}()"/>
//     public bool Is<T>() where T : IAPInstance
//         => TypeMap.Is<T>();
//
//     /// <inheritdoc cref="TypeMap.Is{T}(out T?)"/>
//     public bool Is<T>([NotNullWhen(true)] out T? instance) where T : IAPInstance
//         => TypeMap.Is(out instance);
//
//     /// <inheritdoc cref="TypeMap.As{T}"/>
//     public T As<T>() where T : IAPInstance
//         => TypeMap.As<T>();
// }

public class TypeMap
{
    /// <summary>
    /// Live set of all unique ActivityStreams types represented by this object.
    /// </summary>
    /// <seealso cref="DotNetTypes"/>
    public IReadOnlySet<string> ASTypes => throw new NotImplementedException();

    /// <summary>
    /// Live set of all unique .NET types represented by the object.
    /// This may be a subset of ASTypes.
    /// </summary>
    /// <seealso cref="ASTypes"/>
    public IReadOnlySet<Type> DotNetTypes => throw new NotImplementedException();

    /// <summary>
    /// JSON-LD context in use for this object graph.
    /// </summary>
    public JsonLDContext LDContexts => throw new NotImplementedException();

    /// <summary>
    /// Checks if the object contains a particular type entity.
    /// </summary>
    public bool IsEntity<T>() where T : ASBase
        => throw new NotImplementedException();

    /// <summary>
    /// Checks if the object contains a particular type entity.
    /// If so, then the instance of that type is extracted and returned.
    /// </summary>
    /// <seealso cref="IsEntity{T}()" />
    /// <seealso cref="AsEntity{T}" />
    public bool IsEntity<T>([NotNullWhen(true)] out T? instance) where T : ASBase
        => throw new NotImplementedException();

    /// <summary>
    /// Gets an entity representing the object as type T.
    /// </summary>
    /// <remarks>
    /// This function will not extend the object to include a new type.
    /// To safely convert to an instance that *might* be present, use Is().
    /// </remarks>
    /// <seealso cref="IsEntity{T}(out T?)" />
    /// <throws cref="ArgumentException">If the object is not of type T</throws>
    public T AsEntity<T>() where T : ASBase
        => throw new NotImplementedException();
    
    /// <summary>
    /// Checks if the graph contains a particular type.
    /// </summary>
    public bool IsType<T>() where T : ASType
        => throw new NotImplementedException();
    
    /// <summary>
    /// Checks if the graph contains a particular type.
    /// If so, then the instance of that type is extracted and returned.
    /// </summary>
    /// <seealso cref="IsType{T}()" />
    /// <seealso cref="AsType{T}" />
    public bool IsType<T>([NotNullWhen(true)] out T? instance) where T : ASType
        => throw new NotImplementedException();

    /// <summary>
    /// Gets an object representing the graph as type T.
    /// </summary>
    /// <remarks>
    /// This function will not extend the object to include a new type.
    /// To safely convert to an instance that *might* be present, use Is().
    /// </remarks>
    /// <seealso cref="IsType{T}(out T?)" />
    /// <throws cref="ArgumentException">If the object is not of type T</throws>
    public T AsType<T>() where T : ASType
        => throw new NotImplementedException();

    /// <summary>
    /// Adds a new typed instance to the object.
    /// </summary>
    /// <remarks>
    /// This method is internal, as it should only be called by ASType constructor.
    /// User code should instead add a new type by passing an existing TypeMap into the constructor.
    /// This is not a technical limitation, but rather an intentional choice to avoid merge logic by making object graphs append-only.
    /// TODO make this generic, if compatible with upstream code.
    /// </remarks>
    /// <throws cref="InvalidOperationException">If an object of this type already exists in the graph</throws>
    internal void Add<T>(T instance) where T : ASBase
        => throw new NotImplementedException();
}