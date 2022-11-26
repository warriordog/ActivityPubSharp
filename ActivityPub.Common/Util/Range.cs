using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Common.Util;

// TODO implement deserialization

/// <summary>
/// Synthetic wrapper to handle polymorphic property ranges
/// </summary>
public class Range<T1, T2>
{
    public bool IsType1 { get; protected set; }
    private T1? _value1;
    
    public bool IsType2 { get; protected set; }
    private T2? _value2;

    public bool TryGetAsType1([NotNullWhen(true)] out T1? value1)
    {
        if (IsType1)
        {
            value1 = _value1!;
            return true;
        }

        value1 = default;
        return false;
    }

    public bool TryGetAsType2([NotNullWhen(true)] out T2? value2)
    {
        if (IsType2)
        {
            value2 = _value2!;
            return true;
        }

        value2 = default;
        return false;
    }

    public virtual void SetAsType1(T1 value1)
    {
        _value1 = value1;
        IsType1 = true;
        IsType2 = false;
    }
    public virtual void SetAsType2(T2 value2)
    {
        _value2 = value2;
        IsType1 = false;
        IsType2 = true;
    }
}

public class Range<T1, T2, T3> : Range<T1, T2>
{
    public bool IsType3 { get; protected set; }
    private T3? _value3;

    public bool TryGetAsType3([NotNullWhen(true)] out T3? value3)
    {
        if (IsType3)
        {
            value3 = _value3!;
            return true;
        }

        value3 = default;
        return false;
    }

    public override void SetAsType1(T1 value1)
    {
        base.SetAsType1(value1);
        IsType3 = false;
    }

    public override void SetAsType2(T2 value2)
    {
        base.SetAsType2(value2);
        IsType3 = false;
    }

    public virtual void SetAsType3(T3 value3)
    {
        _value3 = value3;
        IsType1 = false;
        IsType2 = false;
        IsType3 = true;
    } 
}