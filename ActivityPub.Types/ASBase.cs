// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion.Overrides;
using InternalUtils;

namespace ActivityPub.Types;

/// <summary>
///     Base type for AS entity classes.
///     Subtypes should derive from <see cref="ASBase{T}" /> instead.
/// </summary>
public abstract class ASBase
{
    protected ASBase(Type nonEntityType, bool isValueSerialized)
    {
        NonEntityType = nonEntityType;
        IsValueSerialized = isValueSerialized;
    }

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
    internal bool IsValueSerialized { get; }

    /// <summary>
    ///     Creates an instance of the non-entity type that can wrap this entity.
    ///     Internal use only - this requires careful use.
    ///     Will cause graph corruption if called with the wrong TypeMap.
    /// </summary>
    /// <param name="typeMap">TypeMap that contains this entity</param>
    internal abstract ASType CreateTypeInstanceBase(TypeMap typeMap);
}

/// <summary>
///     Base type for AS entity classes.
///     Each subtype should have a matching non-entity class that derives from <see cref="ASType" /> and contains a new(TypeMap) constructor.
/// </summary>
/// <remarks>
///     This is a synthetic type created to help adapt ActivityStreams to the .NET object model.
///     It does not exist in the ActivityStreams standard.
/// </remarks>
/// <typeparam name="TType">Type of the matching non-entity class</typeparam>
/// <seealso cref="ASType" />
public abstract class ASBase<TType> : ASBase
    where TType : ASType
{
    // Expensive reflection / code-gen operations are static to reduce overhead.
    private static readonly bool ImplementsValueSerialized = typeof(TType).IsAssignableToGenericType(typeof(IJsonValueSerialized<>));
    private static readonly Func<TypeMap, TType> TypeConstructor = TypeUtils.CreateDynamicConstructor<TypeMap, TType>();

    /// <summary>
    ///     Creates a new entity.
    /// </summary>
    protected ASBase() : base(typeof(TType), ImplementsValueSerialized) {}

    internal sealed override ASType CreateTypeInstanceBase(TypeMap typeMap) => TypeConstructor(typeMap);
}