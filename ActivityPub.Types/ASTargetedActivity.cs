using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// Synthetic base for activities which require a target.
/// Implemented to overload nullability of <see cref="ASActivity.Target"/>
/// </summary>
public abstract class ASTargetedActivity : ASTransitiveActivity
{
    private Linkable<ASObject>? _targetImpl;
    protected ASTargetedActivity(string type) : base(type) {}

    /// <inheritdoc cref="ASActivity.Target"/>
    /// <seealso href="https://www.w3.org/TR/activitypub/#client-addressing"/>
    public required new Linkable<ASObject> Target
    {
        get => base.Target!;
        set => base.Target = value;
    }

    // This is such a hack but I can't think of any other option
    protected override Linkable<ASObject>? TargetImpl
    {
        get => _targetImpl;
        set => _targetImpl = value ?? throw new NotSupportedException("ASTargetedActivity.Target cannot be set to null");
    }
}