namespace ActivityPub.Types;

/// <summary>
/// Instances of IntransitiveActivity are a subtype of Activity representing intransitive actions.
/// The object property is therefore inappropriate for these activities. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-intransitiveactivity"/>
public class ASIntransitiveActivity : ASActivity
{
    public const string IntransitiveActivityType = "IntransitiveActivity";
    public ASIntransitiveActivity(string type = IntransitiveActivityType) : base(type) {}
}