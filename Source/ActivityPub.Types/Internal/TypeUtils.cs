// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Internal;

internal static class TypeUtils
{
    internal static Func<T> CreateDynamicConstructor<T>()
    {
        var dynamicMethod = typeof(T).CreateDynamicConstructor();
        return dynamicMethod.CreateDelegate<Func<T>>();
    }

    internal static Func<T>? TryCreateDynamicConstructor<T>()
    {
        var dynamicMethod = typeof(T).TryCreateDynamicConstructor();
        return dynamicMethod?.CreateDelegate<Func<T>>();
    }

    internal static Func<TParam, T> CreateDynamicConstructor<TParam, T>()
    {
        var dynamicMethod = typeof(T).CreateDynamicConstructor(typeof(TParam));
        return dynamicMethod.CreateDelegate<Func<TParam, T>>();
    }

    internal static Func<TParam, T>? TryCreateDynamicConstructor<TParam, T>()
    {
        var dynamicMethod = typeof(T).TryCreateDynamicConstructor(typeof(TParam));
        return dynamicMethod?.CreateDelegate<Func<TParam, T>>();
    }

    internal static Func<TP1, TP2, T> CreateDynamicConstructor<TP1, TP2, T>()
    {
        var dynamicMethod = typeof(T).CreateDynamicConstructor(typeof(TP1), typeof(TP2));
        return dynamicMethod.CreateDelegate<Func<TP1, TP2, T>>();
    }

    internal static Func<TP1, TP2, T>? TryCreateDynamicConstructor<TP1, TP2, T>()
    {
        var dynamicMethod = typeof(T).TryCreateDynamicConstructor(typeof(TP1), typeof(TP2));
        return dynamicMethod?.CreateDelegate<Func<TP1, TP2, T>>();
    }

    internal static Func<TP1, TP2, TP3, T> CreateDynamicConstructor<TP1, TP2, TP3, T>()
    {
        var dynamicMethod = typeof(T).CreateDynamicConstructor(typeof(TP1), typeof(TP2), typeof(TP3));
        return dynamicMethod.CreateDelegate<Func<TP1, TP2, TP3, T>>();
    }

    internal static Func<TP1, TP2, TP3, T>? TryCreateDynamicConstructor<TP1, TP2, TP3, T>()
    {
        var dynamicMethod = typeof(T).TryCreateDynamicConstructor(typeof(TP1), typeof(TP2), typeof(TP3));
        return dynamicMethod?.CreateDelegate<Func<TP1, TP2, TP3, T>>();
    }

    internal static Func<TP1, TP2, TP3, TP4, T> CreateDynamicConstructor<TP1, TP2, TP3, TP4, T>()
    {
        var dynamicMethod = typeof(T).CreateDynamicConstructor(typeof(TP1), typeof(TP2), typeof(TP3), typeof(TP4));
        return dynamicMethod.CreateDelegate<Func<TP1, TP2, TP3, TP4, T>>();
    }

    internal static Func<TP1, TP2, TP3, TP4, T>? TryCreateDynamicConstructor<TP1, TP2, TP3, TP4, T>()
    {
        var dynamicMethod = typeof(T).TryCreateDynamicConstructor(typeof(TP1), typeof(TP2), typeof(TP3), typeof(TP4));
        return dynamicMethod?.CreateDelegate<Func<TP1, TP2, TP3, TP4, T>>();
    }
}