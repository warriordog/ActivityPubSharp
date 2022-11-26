namespace ActivityPub.Common.Types;

/// <summary>
/// Instances of IntransitiveActivity are a subtype of Activity representing intransitive actions.
/// The object property is therefore inappropriate for these activities. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-intransitiveactivity"/>
public class ASIntransitiveActivity : ASActivity
{
    public override string ASContext => "https://www.w3.org/ns/activitystreams#IntransitiveActivity";
    public override string Type => "IntransitiveActivity";
}