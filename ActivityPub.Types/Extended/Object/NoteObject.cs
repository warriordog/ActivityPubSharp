namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a short written work typically less than a single paragraph in length. 
/// </summary>
public class NoteObject : ASObject
{
    public const string NoteType = "Note";
    public NoteObject(string type = NoteType) : base(type) {}
}