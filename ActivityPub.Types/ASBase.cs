// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion.Overrides;
using InternalUtils;

namespace ActivityPub.Types;

// TODO placeholder! consider removing it.

/// <summary>
/// Base type for AS entity classes.
/// Subtypes should derive from <see cref="ASBase{T}"/> instead.
/// </summary>
public abstract class ASBase
{
    /// <summary>
    /// Name of the ActivityStreams type that this object implements.
    /// Will be null for synthetic types.
    /// For the full list of names in the object graph, use <see cref="TypeMap.ASTypes"/>.
    /// </summary>
    public string? ASTypeName { get; }


    protected ASBase(string? asTypeName, Type nonEntityType, bool isValueSerialized)
    {
        ASTypeName = asTypeName;
        NonEntityType = nonEntityType;
        IsValueSerialized = isValueSerialized;
    }


    /// <summary>
    /// Creates an instance of the non-entity type that can wrap this entity.
    /// Internal use only - this requires careful use.
    /// Will cause graph corruption if called with the wrong TypeMap.
    /// </summary>
    /// <param name="typeMap">TypeMap that contains this entity</param>
    internal abstract ASType CreateTypeInstanceBase(TypeMap typeMap);

    internal Type NonEntityType { get; }
    internal bool IsValueSerialized { get; }
}

/// <summary>
/// Base type for AS entity classes.
/// Each subtype should have a matching non-entity class that derives from <see cref="ASType"/> and contains a new(TypeMap) constructor.
/// </summary>
/// <remarks>
/// This is a synthetic type created to help adapt ActivityStreams to the .NET object model.
/// It does not exist in the ActivityStreams standard.
/// </remarks>
/// <typeparam name="TType">Type of the matching non-entity class</typeparam>
/// <seealso cref="ASType"/>
public abstract class ASBase<TType> : ASBase
    where TType : ASType
{
    // Expensive check, but its static so constant cost (once for each entity type)
    private static readonly bool ImplementsValueSerialized = typeof(TType).IsAssignableToGenericType(typeof(IJsonValueSerialized<>));

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
        : base(asTypeName, typeof(TType), ImplementsValueSerialized) {}

    private static readonly Func<TypeMap, TType> TypeConstructor = TypeUtils.CreateDynamicConstructor<TypeMap, TType>();
    internal sealed override ASType CreateTypeInstanceBase(TypeMap typeMap) => TypeConstructor(typeMap);
}