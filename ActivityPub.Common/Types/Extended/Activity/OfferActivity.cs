namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is offering the object.
/// If specified, the target indicates the entity to which the object is being offered. 
/// </summary>
public class OfferActivity : ASTransitiveActivity
{
    public OfferActivity(string type = "Offer") : base(type) {}
}