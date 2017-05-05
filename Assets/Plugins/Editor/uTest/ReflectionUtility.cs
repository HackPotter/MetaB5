using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public enum MethodEnumerationOrder
{
    BaseTypeFirst,
    DerivedTypeFirst
}

public static class ReflectionUtility
{
    public static IEnumerable<MethodInfo> GetAllMethods<T>(this Type t, MethodEnumerationOrder methodEnumerationOrder, bool includeAbstractMethods=true) where T : Attribute
    {
        if (t == null)
        {
            return Enumerable.Empty<MethodInfo>();
        }

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        switch (methodEnumerationOrder)
        {
            case MethodEnumerationOrder.BaseTypeFirst:
                return GetAllMethods<T>(t.BaseType, methodEnumerationOrder, includeAbstractMethods).Concat(t.GetMethods(flags).Where((m) => (includeAbstractMethods || !m.IsAbstract) && m.IsDefined(typeof(T), false)));
            case MethodEnumerationOrder.DerivedTypeFirst:
                return t.GetMethods(flags).Where((m) => (includeAbstractMethods || !m.IsAbstract) && m.IsDefined(typeof(T), false)).Concat(GetAllMethods<T>(t.BaseType, methodEnumerationOrder, includeAbstractMethods));
            default:
                return GetAllMethods<T>(t.BaseType, methodEnumerationOrder, includeAbstractMethods).Concat(t.GetMethods(flags).Where((m) => (includeAbstractMethods || !m.IsAbstract) && m.IsDefined(typeof(T), false)));
        }
    }

    public static IEnumerable<MethodInfo> GetAllMethods(this Type t)
    {
        if (t == null)
        {
            return Enumerable.Empty<MethodInfo>();
        }

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        return t.GetMethods(flags).Concat(GetAllMethods(t.BaseType));
    }
}

