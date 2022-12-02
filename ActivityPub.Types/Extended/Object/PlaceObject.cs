namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a logical or physical location.
/// </summary>
public class PlaceObject : ASObject
{
    public const string PlaceType = "Place";
    public PlaceObject(string type = PlaceType) : base(type) {}
    
    /// <summary>
    /// Indicates the accuracy of position coordinates on a Place objects.
    /// Expressed in properties of percentage. e.g. "94.0" means "94.0% accurate". 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attachment"/>
    public float? Accuracy { get; set; }
    
    /// <summary>
    /// Indicates the altitude of a place.
    /// The measurement units is indicated using the units property.
    /// If units is not specified, the default is assumed to be "m" indicating meters. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-altitude"/>
    public float? Altitude { get; set; }
    
    /// <summary>
    /// The latitude of a place.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-latitude"/>
    public float? Latitude { get; set; }
    
    /// <summary>
    /// The longitude of a place.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-longitude"/>
    public float? Longitude { get; set; }
    
    /// <summary>
    /// The radius from the given latitude and longitude for a Place.
    /// The units is expressed by the units property.
    /// If units is not specified, the default is assumed to be "m" indicating "meters". 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-radius"/>
    public float? Radius { get; set; }
    
    /// <summary>
    /// Specifies the measurement units for the radius and altitude properties on a Place object.
    /// If not specified, the default is assumed to be "m" for "meters". 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-units"/>
    public string? Units { get; set; }
}