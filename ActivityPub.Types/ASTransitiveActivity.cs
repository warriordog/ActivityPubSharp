using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// Base type for all activities that are not intransitive
/// </summary>
/// <remarks>
/// This is a synthetic 
/// </remarks>
public abstract class ASTransitiveActivity : ASActivity
{
    protected ASTransitiveActivity(string type) : base(type) {}

    /// <summary>
    /// Describes the direct object of the activity.
    /// For instance, in the activity "John added a movie to his wishlist", the object of the activity is the movie added. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object"/>
    public LinkableList<ASObject> Object { get; set; } = new();
}