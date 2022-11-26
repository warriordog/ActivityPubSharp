namespace ActivityPub.Common.Types.Extended.Object;

/// <summary>
/// A Tombstone represents a content object that has been deleted.
/// It can be used in Collections to signify that there used to be an object at this position, but it has been deleted.
/// </summary>
public class TombstoneObject : ASObject
{
    public TombstoneObject(string type = "Tombstone") : base(type) {}
    
    /// <summary>
    /// On a Tombstone object, the formerType property identifies the type of the object that was deleted.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-formerType"/>
    public string? FormerType { get; set; }
    
    /// <summary>
    /// On a Tombstone object, the deleted property is a timestamp for when the object was deleted. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-deleted"/>
    public DateTime? Deleted { get; set; }
}