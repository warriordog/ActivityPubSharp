using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Common.Util;

// TODO needs a refactor - Range<T1, T2, T3> should not be assignable to Range<T1, T2>.
// It should be possible to just invert the hierarchy - Range<2> is a valid implementation of Range<3> but not the other way around.

/// <summary>
/// Synthetic wrapper to handle polymorphic property ranges.
/// </summary>
public class Range<T1, T2>
{
    public bool IsType1 { get; protected set; }
    protected T1? Value1 { get; private set; }

    public bool IsType2 { get; protected set; }
    protected T2? Value2 { get; private set; }

    public Range(T1 value1)
    {
        Value1 = value1;
        IsType1 = true;
        IsType2 = false;
    }
    public Range(T2 value2)
    {
        Value2 = value2;
        IsType1 = false;
        IsType2 = true;
    }
    protected Range()
    {
        IsType1 = false;
        IsType2 = false;
    }

    public bool TryGetAsType1([NotNullWhen(true)] out T1? value1)
    {
        if (IsType1)
        {
            value1 = Value1!;
            return true;
        }

        value1 = default;
        return false;
    }

    public bool TryGetAsType2([NotNullWhen(true)] out T2? value2)
    {
        if (IsType2)
        {
            value2 = Value2!;
            return true;
        }

        value2 = default;
        return false;
    }

    public virtual void Set(T1 value1)
    {
        Value1 = value1;
        IsType1 = true;
        IsType2 = false;
    }
    public virtual void Set(T2 value2)
    {
        Value2 = value2;
        IsType1 = false;
        IsType2 = true;
    }

    public static implicit operator T1?(Range<T1, T2> range) => range.Value1;
    public static implicit operator Range<T1, T2>(T1 value1) => new(value1);
    
    public static implicit operator T2?(Range<T1, T2> range) => range.Value2;
    public static implicit operator Range<T1, T2>(T2 value2) => new(value2);
}

public class Range<T1, T2, T3> : Range<T1, T2>
{
    public bool IsType3 { get; protected set; }
    protected T3? Value3 { get; private set; }

    public Range(T1 value1) : base(value1)
    {
        IsType3 = false;
    }
    public Range(T2 value2) : base(value2)
    {
        IsType3 = false;
    }
    public Range(T3 value3)
    {
        Value3 = value3;
        IsType3 = true;
    }
    protected Range()
    {
        IsType3 = false;
    }
    
    public bool TryGetAsType3([NotNullWhen(true)] out T3? value3)
    {
        if (IsType3)
        {
            value3 = Value3!;
            return true;
        }

        value3 = default;
        return false;
    }

    public override void Set(T1 value1)
    {
        base.Set(value1);
        IsType3 = false;
    }

    public override void Set(T2 value2)
    {
        base.Set(value2);
        IsType3 = false;
    }

    public virtual void Set(T3 value3)
    {
        Value3 = value3;
        IsType1 = false;
        IsType2 = false;
        IsType3 = true;
    } 

    public static implicit operator T1?(Range<T1, T2, T3> range) => range.Value1;
    public static implicit operator Range<T1, T2, T3>(T1 value1) => new(value1);
    
    public static implicit operator T2?(Range<T1, T2, T3> range) => range.Value2;
    public static implicit operator Range<T1, T2, T3>(T2 value2) => new(value2);
    
    public static implicit operator T3?(Range<T1, T2, T3> range) => range.Value3;
    public static implicit operator Range<T1, T2, T3>(T3 value3) => new(value3);
}