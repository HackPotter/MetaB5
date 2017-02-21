using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Utilities for uVerifier.
/// <br/>
/// Contains methods for finding Members of GameObjects and MonoBehaviours that have been tagged with ConditionVerifierAttribute.
/// </summary>
public static class VerifierUtils
{
    /// <summary>
    /// Finds all members of the given GameObject's MonoBehaviours that have been tagged with a ConditionVerifierAttribute of the specified Type T.
    /// </summary>
    /// <typeparam name="T">The Type of ConditionVerifierAttribute with which returned members have been tagged.</typeparam>
    /// <param name="gameObject">The GameObject on which to search for members.</param>
    /// <returns>A List of MemberInfo representing the members found on the GameObject's MonoBehaviours tagged with the T attribute.</returns>
    public static List<MemberInfo> GetMembersWithAttribute<T>(GameObject gameObject) where T : ConditionVerifierAttribute
    {
        Asserter.NotNull(gameObject);
        return GetMembersWithAttribute(gameObject, typeof(T));
    }

    /// <summary>
    /// Finds all members of the given MonoBehaviour that have been tagged with a ConditionVerifierAttribute of the specified Type T.
    /// </summary>
    /// <typeparam name="T">The Type of ConditionVerifierAttribute with which returned members have been tagged.</typeparam>
    /// <param name="component">The MonoBehaviour on which to search for members.</param>
    /// <returns>A List of MemberInfo representing the members found on the MonoBehaviour tagged with the T attribute.</returns>
    public static List<MemberInfo> GetMembersWithAttribute<T>(MonoBehaviour component) where T : ConditionVerifierAttribute
    {
        Asserter.NotNull(component);
        return GetMembersWithAttribute(component, typeof(T));
    }

    /// <summary>
    /// Finds all members of the given GameObject's MonoBehaviours that have been tagged with a ConditionVerifierAttribute of a specified type.
    /// </summary>
    /// <param name="gameObject">The GameObject on which to search for members.</param>
    /// <param name="attributeType">The Type of ConditionVerifierAttribute with which returned members have been tagged.</param>
    /// <returns>A List of MemberInfo representing the members found on the GameObject's MonoBehaviours tagged with the given attribute type.</returns>
    public static List<MemberInfo> GetMembersWithAttribute(GameObject gameObject, Type attributeType)
    {
        Asserter.NotNull(gameObject);
        Asserter.NotNull(attributeType);
        List<MemberInfo> membersWithAttribute = new List<MemberInfo>();
        
        MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour component in components)
        {
            if (!component)
            {
                // stupid; missing components will be null but still show up.
                continue;
            }
            membersWithAttribute.AddRange(GetMembersWithAttribute(component,attributeType));
        }

        return membersWithAttribute;
    }

    /// <summary>
    /// Finds all members of the given MonoBehaviour that have been tagged with a ConditionVerifierAttribute of the specified Type T.
    /// </summary>
    /// <param name="component">The MonoBehaviour on which to search for members.</param>
    /// <param name="attributeType">The Type of ConditionVerifierAttribute with which returned members have been tagged.</param>
    /// <returns>A List of MemberInfo representing the members found on the MonoBehaviour tagged with the given attribute type.</returns>
    public static List<MemberInfo> GetMembersWithAttribute(MonoBehaviour component, Type attributeType)
    {
        Asserter.NotNull(component);
        Asserter.NotNull(attributeType);

        Type componentType = component.GetType();
        MemberInfo[] members = componentType.GetMembers();

        List<MemberInfo> membersWithAttribute = new List<MemberInfo>();

        foreach (MemberInfo memberInfo in members)
        {
            if (MemberInfoHasConditionAttribute(memberInfo,attributeType))
            {
                membersWithAttribute.Add(memberInfo);
            }
        }

        if (MemberInfoHasConditionAttribute(component.GetType(), attributeType))
        {
            membersWithAttribute.Add(component.GetType());
        }

        return membersWithAttribute;
    }

    /// <summary>
    /// Returns true if the given MemberInfo has been tagged with a ConditionVerifierAttribute of the type T.
    /// </summary>
    /// <typeparam name="T">The Type of ConditionVerifierAttribute to look for.</typeparam>
    /// <param name="memberInfo">The MemberInfo on which to search for ConditionVerifierAttributes.</param>
    /// <returns>True if the given MemberInfo has been tagged with a ConditionVerifierAttribute of type T.</returns>
    public static bool MemberInfoHasConditionAttribute<T>(MemberInfo memberInfo) where T : ConditionVerifierAttribute
    {
        Asserter.NotNull(memberInfo);

        return MemberInfoHasConditionAttribute(memberInfo, typeof(T));
    }

    /// <summary>
    /// Returns true if the given MemberInfo has been tagged with a ConditionVerifierAttribute of the type T.
    /// </summary>
    /// <param name="memberInfo">The MemberInfo on which to search for ConditionVerifierAttributes.</param>
    /// <param name="attributeType">The Type of ConditionVerifierAttribute to look for.</param>
    /// <returns>True if the given MemberInfo has been tagged with a ConditionVerifierAttribute of the specified type.</returns>
    public static bool MemberInfoHasConditionAttribute(MemberInfo memberInfo, Type attributeType)
    {
        Asserter.NotNull(memberInfo);
        Asserter.NotNull(attributeType);
        return memberInfo.GetCustomAttributes(attributeType, true).Length > 0 ? true : false;
    }

    /// <summary>
    /// Returns all ConditionVerifierAttributes present on the given MemberInfo.
    /// </summary>
    /// <typeparam name="T">The Type of ConditionVerifierAttributes to return.</typeparam>
    /// <param name="memberInfo">The MemberInfo on which to search for ConditionVerifierAttributes.</param>
    /// <returns>An array of type T containing the ConditionVerifierAttributes found on the given MemberInfo.</returns>
    public static T[] GetAttributesOfType<T>(MemberInfo memberInfo) where T : ConditionVerifierAttribute
    {
        Asserter.NotNull(memberInfo);
        return (memberInfo.GetCustomAttributes(typeof(T), true) as T[]);
    }

    /// <summary>
    /// Returns true if the given object is null. Also checks for Unity's non-standard null conditions.
    /// </summary>
    /// <param name="obj">The object to be checked.</param>
    /// <returns>True if the System.Object is null or the object is a UnityEngine.Object and it is null.</returns>
    public static bool IsNull(object obj)
    {
        if (obj == null)
        {
            return true;
        }
        else if (obj is UnityEngine.Object)
        {
            UnityEngine.Object objectAsUnityObject = obj as UnityEngine.Object;
            if (!objectAsUnityObject)
            {
                return true;
            }
        }

        return false;
    }
}

