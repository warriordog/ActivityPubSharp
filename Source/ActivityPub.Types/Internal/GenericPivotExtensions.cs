using System.Reflection;

namespace ActivityPub.Types.Internal;

internal static class GenericPivotExtensions
{
    /// <summary>
    ///     Creates a <see langword="delegate"/> that calls the provided method using a specified generic type.
    ///     Additional generic overloads are automatically constructed as-needed, and cached for the lifetime of the delegate.
    ///     An object can be provided to bind instance methods.
    /// </summary>
    /// <exception cref="ArgumentException">When <code>method</code> is <see langword="static"/> but <code>instance</code> is not <see langword="null"/></exception>
    /// <exception cref="ArgumentException">When <code>method</code> is not <see langword="static"/> but <code>instance</code> is <see langword="null"/></exception>
    internal static Func<Type, TResult> CreateGenericPivotFunc<TResult>(this MethodInfo method, object? instance = null)
    {
        CheckMethodInstanceAlignment(method, instance);

        var cache = new Dictionary<Type, Func<TResult>>();
        return type =>
        {
            if (!cache.TryGetValue(type, out var genericDelegate))
            {
                genericDelegate = method
                    .MakeGenericMethod(type)
                    .CreateDelegate<Func<TResult>>(instance);
                cache[type] = genericDelegate;
            }

            return genericDelegate();
        };
    }

    /// <inheritdoc cref="CreateGenericPivotFunc{TResult}"/>
    internal static Func<Type, TArg, TResult> CreateGenericPivotFunc<TArg, TResult>(this MethodInfo method, object? instance = null)
    {
        CheckMethodInstanceAlignment(method, instance);
        
        var cache = new Dictionary<Type, Func<TArg, TResult>>();
        return (type, arg) =>
        {
            if (!cache.TryGetValue(type, out var genericDelegate))
            {
                genericDelegate = method
                    .MakeGenericMethod(type)
                    .CreateDelegate<Func<TArg, TResult>>(instance);
                cache[type] = genericDelegate;
            }

            return genericDelegate(arg);
        };
    }

    /// <inheritdoc cref="CreateGenericPivotFunc{TResult}"/>
    internal static Func<Type, TArg1, TArg2, TResult> CreateGenericPivotFunc<TArg1, TArg2, TResult>(this MethodInfo method, object? instance = null)
    {
        CheckMethodInstanceAlignment(method, instance);
        
        var cache = new Dictionary<Type, Func<TArg1, TArg2, TResult>>();
        return (type, arg1, arg2) =>
        {
            if (!cache.TryGetValue(type, out var genericDelegate))
            {
                genericDelegate = method
                    .MakeGenericMethod(type)
                    .CreateDelegate<Func<TArg1, TArg2, TResult>>(instance);
                cache[type] = genericDelegate;
            }

            return genericDelegate(arg1, arg2);
        };
    }

    /// <inheritdoc cref="CreateGenericPivotFunc{TResult}"/>
    internal static Func<Type, TArg1, TArg2, TArg3, TResult> CreateGenericPivotFunc<TArg1, TArg2, TArg3, TResult>(this MethodInfo method, object? instance = null)
    {
        CheckMethodInstanceAlignment(method, instance);
        
        var cache = new Dictionary<Type, Func<TArg1, TArg2, TArg3, TResult>>();
        return (type, arg1, arg2, arg3) =>
        {
            if (!cache.TryGetValue(type, out var genericDelegate))
            {
                genericDelegate = method
                    .MakeGenericMethod(type)
                    .CreateDelegate<Func<TArg1, TArg2, TArg3, TResult>>(instance);
                cache[type] = genericDelegate;
            }

            return genericDelegate(arg1, arg2, arg3);
        };
    }

    /// <inheritdoc cref="CreateGenericPivotFunc{TResult}"/>
    internal static Func<Type, TArg1, TArg2, TArg3, TArg4, TResult> CreateGenericPivotFunc<TArg1, TArg2, TArg3, TArg4, TResult>(this MethodInfo method, object? instance = null)
    {
        CheckMethodInstanceAlignment(method, instance);
        
        var cache = new Dictionary<Type, Func<TArg1, TArg2, TArg3, TArg4, TResult>>();
        return (type, arg1, arg2, arg3, arg4) =>
        {
            if (!cache.TryGetValue(type, out var genericDelegate))
            {
                genericDelegate = method
                    .MakeGenericMethod(type)
                    .CreateDelegate<Func<TArg1, TArg2, TArg3, TArg4, TResult>>(instance);
                cache[type] = genericDelegate;
            }

            return genericDelegate(arg1, arg2, arg3, arg4);
        };
    }
    
    /// <inheritdoc cref="CreateGenericPivotFunc{TResult}"/>
    internal static Action<Type> CreateGenericPivotAction(this MethodInfo method, object? instance = null)
    {
        CheckMethodInstanceAlignment(method, instance);

        var cache = new Dictionary<Type, Action>();
        return type =>
        {
            if (!cache.TryGetValue(type, out var genericDelegate))
            {
                genericDelegate = method
                    .MakeGenericMethod(type)
                    .CreateDelegate<Action>(instance);
                cache[type] = genericDelegate;
            }

            genericDelegate();
        };
    }

    /// <inheritdoc cref="CreateGenericPivotAction{TArg}"/>
    internal static Action<Type, TArg> CreateGenericPivotAction<TArg>(this MethodInfo method, object? instance = null)
    {
        CheckMethodInstanceAlignment(method, instance);
        
        var cache = new Dictionary<Type, Action<TArg>>();
        return (type, arg) =>
        {
            if (!cache.TryGetValue(type, out var genericDelegate))
            {
                genericDelegate = method
                    .MakeGenericMethod(type)
                    .CreateDelegate<Action<TArg>>(instance);
                cache[type] = genericDelegate;
            }

            genericDelegate(arg);
        };
    }

    /// <inheritdoc cref="CreateGenericPivotAction{TArg}"/>
    internal static Action<Type, TArg1, TArg2> CreateGenericPivotAction<TArg1, TArg2>(this MethodInfo method, object? instance = null)
    {
        CheckMethodInstanceAlignment(method, instance);
        
        var cache = new Dictionary<Type, Action<TArg1, TArg2>>();
        return (type, arg1, arg2) =>
        {
            if (!cache.TryGetValue(type, out var genericDelegate))
            {
                genericDelegate = method
                    .MakeGenericMethod(type)
                    .CreateDelegate<Action<TArg1, TArg2>>(instance);
                cache[type] = genericDelegate;
            }

            genericDelegate(arg1, arg2);
        };
    }

    /// <inheritdoc cref="CreateGenericPivotAction{TArg}"/>
    internal static Action<Type, TArg1, TArg2, TArg3> CreateGenericPivotAction<TArg1, TArg2, TArg3>(this MethodInfo method, object? instance = null)
    {
        CheckMethodInstanceAlignment(method, instance);
        
        var cache = new Dictionary<Type, Action<TArg1, TArg2, TArg3>>();
        return (type, arg1, arg2, arg3) =>
        {
            if (!cache.TryGetValue(type, out var genericDelegate))
            {
                genericDelegate = method
                    .MakeGenericMethod(type)
                    .CreateDelegate<Action<TArg1, TArg2, TArg3>>(instance);
                cache[type] = genericDelegate;
            }

            genericDelegate(arg1, arg2, arg3);
        };
    }

    /// <inheritdoc cref="CreateGenericPivotAction{TArg}"/>
    internal static Action<Type, TArg1, TArg2, TArg3, TArg4> CreateGenericPivotAction<TArg1, TArg2, TArg3, TArg4>(this MethodInfo method, object? instance = null)
    {
        CheckMethodInstanceAlignment(method, instance);
        
        var cache = new Dictionary<Type, Action<TArg1, TArg2, TArg3, TArg4>>();
        return (type, arg1, arg2, arg3, arg4) =>
        {
            if (!cache.TryGetValue(type, out var genericDelegate))
            {
                genericDelegate = method
                    .MakeGenericMethod(type)
                    .CreateDelegate<Action<TArg1, TArg2, TArg3, TArg4>>(instance);
                cache[type] = genericDelegate;
            }

            genericDelegate(arg1, arg2, arg3, arg4);
        };
    }

    private static void CheckMethodInstanceAlignment(MethodInfo method, object? instance)
    {
        if (method.IsStatic && instance != null)
            throw new ArgumentException("instance must be null for static method", nameof(instance));

        if (!method.IsStatic && instance == null)
            throw new ArgumentException("instance must be provided for instance methods", nameof(instance));
    }
}