// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a logical or physical location.
/// </summary>
public class PlaceObject : ASObject, IASModel<PlaceObject, PlaceObjectEntity, ASObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Place" types.
    /// </summary>
    public const string PlaceType = "Place";
    static string IASModel<PlaceObject>.ASTypeName => PlaceType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public PlaceObject() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public PlaceObject(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<PlaceObjectEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public PlaceObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public PlaceObject(TypeMap typeMap, PlaceObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<PlaceObjectEntity>();

    static PlaceObject IASModel<PlaceObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private PlaceObjectEntity Entity { get; }

    /// <summary>
    ///     Indicates the accuracy of position coordinates on a Place objects.
    ///     Expressed in properties of percentage. e.g. "94.0" means "94.0% accurate".
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-accuracy" />
    public float? Accuracy
    {
        get => Entity.Accuracy;
        set => Entity.Accuracy = value;
    }

    /// <summary>
    ///     Indicates the altitude of a place.
    ///     The measurement units is indicated using the units property.
    ///     If units is not specified, the default is assumed to be "m" indicating meters.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-altitude" />
    public float? Altitude
    {
        get => Entity.Altitude;
        set => Entity.Altitude = value;
    }

    /// <summary>
    ///     The latitude of a place.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-latitude" />
    public float? Latitude
    {
        get => Entity.Latitude;
        set => Entity.Latitude = value;
    }

    /// <summary>
    ///     The longitude of a place.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-longitude" />
    public float? Longitude
    {
        get => Entity.Longitude;
        set => Entity.Longitude = value;
    }

    /// <summary>
    ///     The radius from the given latitude and longitude for a Place.
    ///     The units is expressed by the units property.
    ///     If units is not specified, the default is assumed to be "m" indicating "meters".
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-radius" />
    public float? Radius
    {
        get => Entity.Radius;
        set => Entity.Radius = value;
    }

    /// <summary>
    ///     Specifies the measurement units for the radius and altitude properties on a Place object.
    ///     If not specified, the default is assumed to be "m" for "meters".
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-units" />
    public string? Units
    {
        get => Entity.Units;
        set => Entity.Units = value;
    }
}

/// <inheritdoc cref="PlaceObject" />
public sealed class PlaceObjectEntity : ASEntity<PlaceObject, PlaceObjectEntity>
{
    /// <inheritdoc cref="PlaceObject.Accuracy" />
    [JsonPropertyName("accuracy")]
    public float? Accuracy { get; set; }

    /// <inheritdoc cref="PlaceObject.Altitude" />
    [JsonPropertyName("altitude")]
    public float? Altitude { get; set; }

    /// <inheritdoc cref="PlaceObject.Latitude" />
    [JsonPropertyName("latitude")]
    public float? Latitude { get; set; }

    /// <inheritdoc cref="PlaceObject.Longitude" />
    [JsonPropertyName("longitude")]
    public float? Longitude { get; set; }

    /// <inheritdoc cref="PlaceObject.Radius" />
    [JsonPropertyName("radius")]
    public float? Radius { get; set; }

    /// <inheritdoc cref="PlaceObject.Units" />
    [JsonPropertyName("units")]
    public string? Units { get; set; }
}