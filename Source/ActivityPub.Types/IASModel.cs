// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.AS;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Util;
using JetBrains.Annotations;

namespace ActivityPub.Types;

/// <summary>
///     Indicates that the class is a convertible ActivityStreams model.
///     This is a low-level base interface; most classes should derive from <see cref="IASModel{TModel,TEntity}"/> or <see cref="IASModel{TModel,TEntity,TBaseModel}"/> instead.
///     Implementations MUST use <see href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/interfaces/explicit-interface-implementation">explicit interface implementations</see> for these values!
/// </summary>
/// <typeparam name="TModel">Type of the implementing class (the type of "this")</typeparam>
public interface IASModel<out TModel>
    where TModel : ASType, IASModel<TModel>
{
    /// <summary>
    ///     Type of the entity associated with this model.
    /// </summary>
    [JsonIgnore]
    public static abstract Type EntityType { get; }
    
    /// <inheritdoc cref="ASEntity.ASTypeName"/>
    [JsonIgnore]
    public static virtual string? ASTypeName => null;

    /// <inheritdoc cref="ASEntity.BaseTypeName"/>
    [JsonIgnore]
    public static virtual string? BaseTypeName => null;
    
    /// <inheritdoc cref="ASEntity.DefiningContext"/>
    [JsonIgnore]
    public static virtual IJsonLDContext DefiningContext => IJsonLDContext.ActivityStreams;
    
    /// <summary>
    ///     Constructs an instance from this type from a pre-populated type graph.
    ///     The provided <see cref="TypeMap"/> instance is guaranteed to include an instance of type <see cref="EntityType"/>.
    /// </summary>
    public static abstract TModel FromGraph(TypeMap typeMap);

    /// <summary>
    ///     Overrides the native type-matching logic.
    ///     This will be called when an object is being converted to check if it contains this type.
    ///     Return <see langword="true"/> to forcibly parse the type, <see langword="true"/> to block parsing, or <see langword="null"/> to resume native logic.
    /// </summary>
    /// <param name="inputJson">JSON message that is being converted. May not be an object.</param>
    /// <param name="typeMap">Type graph that may contain the object. Contains the JSON-LD context and other metadata.</param>
    [PublicAPI]
    public static virtual bool? ShouldConvertFrom(JsonElement inputJson, TypeMap typeMap) => null;
    
    /// <summary>
    ///     AS name of all types that derive from this one.
    /// </summary>
    /// <seealso cref="ASNameTree"/>
    internal static virtual HashSet<string>? DerivedTypeNames => ASNameTree.GetDerivedTypesFor(TModel.ASTypeName);
    static IASModel() => ASNameTree.Add(TModel.ASTypeName, TModel.BaseTypeName);
}

/// <summary>
///     Indicates that the class is a convertible ActivityStreams model.
///     The <see cref="IASModel{TModel}.EntityType"/> property is automatically set to Entity. 
/// </summary>
/// <typeparam name="TModel">Type of the implementing class (the type of <see langword="this"/>)</typeparam>
/// <typeparam name="TEntity">Type of this model's entity</typeparam>
public interface IASModel<out TModel, out TEntity> : IASModel<TModel>
    where TModel : ASType, IASModel<TModel, TEntity>
    where TEntity : ASEntity<TModel, TEntity>
{
    static Type IASModel<TModel>.EntityType { get; } = typeof(TEntity);
}

/// <summary>
///     Indicates that the class is a convertible ActivityStreams model that shadows a base type.
///     The <see cref="IASModel{TModel}.BaseTypeName"/> property is automatically populated from <code>TBaseModel</code>.
///     If the base type does not have a type name, then the property is recursively populated from *its* base type.
/// </summary>
/// <typeparam name="TModel">Type of the implementing class (the type of <see langword="this"/>)</typeparam>
/// <typeparam name="TEntity">Type of this model's entity</typeparam>
/// <typeparam name="TBaseModel">The model's base type</typeparam>
public interface IASModel<out TModel, out TEntity, out TBaseModel> : IASModel<TModel, TEntity>
    where TModel : TBaseModel, IASModel<TModel, TEntity, TBaseModel>
    where TEntity : ASEntity<TModel, TEntity>
    where TBaseModel : ASType, IASModel<TBaseModel>
{
    static string? IASModel<TModel>.BaseTypeName => TBaseModel.ASTypeName ?? TBaseModel.BaseTypeName;
}