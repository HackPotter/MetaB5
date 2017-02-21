using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public static class TypeExtensions
{
    public static IEnumerable<KeyValuePair<FieldInfo, T>> GetFieldsWithAttribute<T>(this Type type) where T : Attribute
    {
        return type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where((fi) => fi.IsDefined(typeof(T), false))
                .Select((fi) => new KeyValuePair<FieldInfo, T>(fi, fi.GetAttribute<T>()))
                .ToArray();
    }

    public static FieldInfo[] GetFieldInfo<T>(this Type type) where T : Attribute
    {
        return type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where((fi) => fi.IsDefined(typeof(T), false)).ToArray();
    }

    public static T GetAttribute<T>(this MemberInfo field, bool inherit=false) where T : Attribute
    {
        if (!field.IsDefined(typeof(T), inherit))
        {
            return null;
        }

        return ((T[])field.GetCustomAttributes(typeof(T), inherit))[0];
    }

    public static T[] GetAttributes<T>(this MemberInfo field, bool inherit = false) where T : Attribute
    {
        if (!field.IsDefined(typeof(T), inherit))
        {
            return new T[0];
        }

        return (T[])field.GetCustomAttributes(typeof(T), inherit);
    }
}

