using System.Reflection.Emit;

namespace ActivityPub.Types.Internal;

internal static class DynamicConstructorExtensions
{ 
    /// <summary>
    ///     Based on https://stackoverflow.com/a/23433748
    ///     More info - https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.dynamicmethod?view=net-7.0
    ///     More info - https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes?view=net-7.0
    /// </summary>
    internal static DynamicMethod CreateDynamicConstructor(this Type type, params Type[] paramTypes)
    {
        var dynamicConstructor = TryCreateDynamicConstructor(type, paramTypes);
        if (dynamicConstructor == null)
            throw new ArgumentException($"Can't create dynamic constructor: {type}({string.Join<Type>(", ", paramTypes)}) does not exist");

        return dynamicConstructor;
    }

    /// <summary>
    ///     Alternate version of CreateDynamicConstructor that safely handles abstract and non-constructable types.
    /// </summary>
    /// <returns>Returns false if the type cannot be constructed</returns>
    internal static DynamicMethod? TryCreateDynamicConstructor(this Type type, params Type[] paramTypes)
    {
        if (paramTypes.Length > 4)
            throw new ArgumentException("Can't create dynamic constructor: no more than 4 parameters can be provided");

        var constructor = type.GetConstructor(paramTypes);
        if (constructor == null)
            return null;

        var dynamicConstructor = new DynamicMethod($"DynamicConstructor_{paramTypes.Length}", type, paramTypes, true);
        var il = dynamicConstructor.GetILGenerator();
        if (paramTypes.Length > 0)
            il.Emit(OpCodes.Ldarg_0);
        if (paramTypes.Length > 1)
            il.Emit(OpCodes.Ldarg_1);
        if (paramTypes.Length > 2)
            il.Emit(OpCodes.Ldarg_2);
        if (paramTypes.Length > 3)
            il.Emit(OpCodes.Ldarg_3);
        il.Emit(OpCodes.Newobj, constructor);
        il.Emit(OpCodes.Ret);
        return dynamicConstructor;
    }
}