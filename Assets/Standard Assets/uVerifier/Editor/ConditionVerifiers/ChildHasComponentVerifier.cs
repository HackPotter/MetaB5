using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// IConditionVerifier for the condition that a given GameObject field, MonoBehaviour field, or MonoBehaviour Type has a child GameObject containing a component of the type specified in
/// the ChildHasComponentAttribute.
/// </summary>
public class ChildHasComponentVerifier : BaseConditionVerifier<ChildHasComponentAttribute>
{
    protected override bool VerifyMember(MonoBehaviour target, MemberInfo member, ChildHasComponentAttribute[] attributes, out FieldViolation[] fieldViolation)
    {
        List<FieldViolation> fieldViolationList = new List<FieldViolation>();
        GameObject gameObjectWithExpectedChildren = GetGameObjectWithExpectedChildren(target, member);

        foreach (ChildHasComponentAttribute attribute in attributes)
        {
            if (gameObjectWithExpectedChildren == null)
            {
                fieldViolationList.Add(new FieldViolation(target.gameObject, null, target.name + "." + member.Name + ": Child of object referenced by field must have " + attribute.ComponentType + ", but is null."));
                continue;
            }
            if (!GameObjectHasChildOfType(gameObjectWithExpectedChildren, attribute.ComponentType))
            {
                fieldViolationList.Add(new FieldViolation(target.gameObject, null, target.name + "." + member.Name + ": Child of object referenced by field must have " + attribute.ComponentType + ", but does not."));
            }
        }

        fieldViolation = fieldViolationList.ToArray();
        if (fieldViolation.Length > 0)
        {
            return true;
        }
        return false;
    }

    private bool GameObjectHasChildOfType(GameObject gameObject, Type componentType)
    {
        return gameObject.GetComponentsInChildren(componentType).Length > 0 ? true : false;
    }

    private GameObject GetGameObjectWithExpectedChildren(MonoBehaviour target, MemberInfo member)
    {
        if (member is FieldInfo)
        {
            object value = (member as FieldInfo).GetValue(target);
            if (!VerifierUtils.IsNull(value))
            {
                if (value is GameObject)
                {
                    return value as GameObject;
                }
                else if (value is MonoBehaviour)
                {
                    return (value as MonoBehaviour).gameObject;
                }
            }
        }

        if (member is Type)
        {
            return target.gameObject;
        }

        return null;
    }
}

