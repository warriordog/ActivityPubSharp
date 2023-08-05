// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.Conversion.Overrides;
using InternalUtils;

namespace ActivityPub.Types.AS;

/// <summary>
///     Base type for AS entity classes.
///     Entities are singletons that contain data for a certain type within an object graph.
/// </summary>
/// <remarks>
///     This is a synthetic type created to help adapt ActivityStreams to the .NET object model.
///     It does not exist in the ActivityStreams standard.
///     Subtypes should derive from <see cref="ASEntity{TType}" /> instead.
/// </remarks>
/// <seealso cref="ASEntity{TType}"/>
public abstract class ASEntity
{
    internal ASEntity(Type nonEntityType) => NonEntityType = nonEntityType;

    /// <summary>
    ///     Name of the ActivityStreams type that this object implements.
    ///     Will be null for synthetic types.
    ///     For the full list of names in the object graph, use <see cref="TypeMap.ASTypes" />.
    /// </summary>
    /// <remarks>
    ///     Entity implementations should override this to opt-in to the functionality.
    /// </remarks>
    public virtual string? ASTypeName => null;

    /// <summary>
    ///     If set, represents a set of AS Type names that should be excluded / removed when this entity is added to a graph.
    ///     Ignored if <see cref="ASTypeName" /> is null.
    ///     <para>
    ///         For example, MentionLink extends Link so the latter should be removed when both are present.
    ///         This does not remove the actual entities - only the type names.
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     Entity implementations should override this to opt-in to the functionality.
    /// </remarks>
    public virtual IReadOnlySet<string>? ReplacesASTypes => null;

    /// <summary>
    ///     Links this entity to the provided TypeMap.
    ///     Can only be called as an initializer.
    /// </summary>
    /// <throws cref="InvalidOperationException">If an entity of this type already exists in the graph</throws>
    public TypeMap TypeMap
    {
        init => value.Add(this);
    }

    internal Type NonEntityType { get; }

    /// <summary>
    ///     Creates an instance of the non-entity type that can wrap this entity.
    ///     Internal use only - this requires careful use.
    ///     Will cause graph corruption if called with the wrong TypeMap.
    /// </summary>
    /// <param name="typeMap">TypeMap that contains this entity</param>
    /// <param name="instance">Instance that was constructed</param>
    internal abstract bool TryCreateTypeInstance(TypeMap typeMap, [NotNullWhen(true)] out ASType? instance);

    /// <summary>
    ///     Returns true if this entity implements IJsonValueSerialized{TThis}.
    ///     Ugly and violates good OOP principles, but necessary for performance.
    ///     Implementations should do this once per type and cache the result.
    /// </summary>
    internal abstract bool IsValueSerialized { get; }
}

/// <inheritdoc cref="ASEntity"/>
/// <typeparam name="TType">Type of the object that extends this type</typeparam>
public abstract class ASEntity<TType> : ASEntity
    where TType : ASType
{
    // Expensive reflection / code-gen operations are static to reduce overhead.
    private static readonly Func<TypeMap, TType>? TypeConstructor = TypeUtils.TryCreateDynamicConstructor<TypeMap, TType>();
    private static readonly bool IsTypeValueSerialized = typeof(TType).IsAssignableToGenericType(typeof(IJsonValueSerialized<>));

    /// <summary>
    ///     Creates a new entity.
    /// </summary>
    protected ASEntity() : base(typeof(TType)) {}

    internal sealed override bool TryCreateTypeInstance(TypeMap typeMap, [NotNullWhen(true)] out ASType? instance)
    {
        if (TypeConstructor != null)
        {
            instance = TypeConstructor(typeMap);
            return true;
        }

        instance = null;
        return false;
    }

    internal sealed override bool IsValueSerialized => IsTypeValueSerialized;
}