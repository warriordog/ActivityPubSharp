// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion.Overrides;
using InternalUtils;

namespace ActivityPub.Types;

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

    /// <summary>
    /// If set, represents a set of AS Type names that should be excluded / removed when this entity is added to a graph.
    /// For example, MentionLink extends Link so the latter should be removed when both are present.
    /// This does not remove the actual entities - only the type names.
    /// </summary>
    public IReadOnlySet<string>? ReplacesASTypes { get; }

    protected ASBase(Type nonEntityType, bool isValueSerialized)
    {
        NonEntityType = nonEntityType;
        IsValueSerialized = isValueSerialized;
        ASTypeName = null;
        ReplacesASTypes = null;
    }

    protected ASBase(Type nonEntityType, bool isValueSerialized, string asTypeName, IReadOnlySet<string>? replacesASTypes)
    {
        NonEntityType = nonEntityType;
        IsValueSerialized = isValueSerialized;
        ASTypeName = asTypeName;
        ReplacesASTypes = replacesASTypes;
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
    /// Creates an anonymous entity and attaches it to a specified type graph.
    /// </summary>
    /// <throws cref="InvalidOperationException">If an entity of this type already exists in the graph</throws>
    protected ASBase(TypeMap typeMap) : this()
        => typeMap.Add(this);

    /// <summary>
    /// Creates a named entity and attaches it to a specified type graph.
    /// </summary>
    /// <throws cref="InvalidOperationException">If an entity of this type already exists in the graph</throws>
    protected ASBase(TypeMap typeMap, string asTypeName, IReadOnlySet<string>? replacesASTypes = null) : this(asTypeName, replacesASTypes)
        => typeMap.Add(this);
    
    /// <summary>
    /// Creates a free-floating anonymous entity, not attached to any graph.
    /// </summary>
    protected ASBase()
        : base(typeof(TType), ImplementsValueSerialized) {}
    
    /// <summary>
    /// Creates a free-floating named entity, not attached to any graph.
    /// </summary>
    protected ASBase(string asTypeName, IReadOnlySet<string>? replacesASTypes = null)
        : base(typeof(TType), ImplementsValueSerialized, asTypeName, replacesASTypes) {}

    private static readonly Func<TypeMap, TType> TypeConstructor = TypeUtils.CreateDynamicConstructor<TypeMap, TType>();
    internal override sealed ASType CreateTypeInstanceBase(TypeMap typeMap) => TypeConstructor(typeMap);
}