namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// A specialization of Offer in which the actor is extending an invitation for the object to the target. 
/// </summary>
public class InviteActivity : OfferActivity
{
    public const string InviteType = "Invite";
    public InviteActivity(string type = InviteType) : base(type) {}
}