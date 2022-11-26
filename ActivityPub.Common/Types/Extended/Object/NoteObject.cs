namespace ActivityPub.Common.Types.Extended.Object;

/// <summary>
/// Represents a short written work typically less than a single paragraph in length. 
/// </summary>
public class NoteObject : ASObject
{
    public NoteObject(string type = "Note") : base(type) {}
}