using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// IConditionVerifier for the condition that a given GameObject field, MonoBehaviour field, or MonoBehaviour Type has a parent GameObject containing a component of the type specified in
/// the ParentHasComponentAttribute.
/// </summary>
public class ParentHasComponentVerifier : BaseConditionVerifier<ParentHasComponentAttribute>
{
    protected override bool VerifyMember(MonoBehaviour target, MemberInfo member, ParentHasComponentAttribute[] attributes, out FieldViolation[] fieldViolation)
    {
        List<FieldViolation> fieldViolationList = new List<FieldViolation>();
        GameObject memberValue = GetGameObjectOfMember(target, member);

        foreach (ParentHasComponentAttribute attribute in attributes)
        {
            if (memberValue == null)
            {
                fieldViolationList.Add(new FieldViolation(target.gameObject, null, target.name + "." + member.Name + ": Parent of object referenced by field must have " + attribute.ComponentType + ", but is null."));
                continue;
            }
            if (!GameObjectHasParentWithComponent(memberValue, attribute.ComponentType))
            {
                fieldViolationList.Add(new FieldViolation(target.gameObject, null, target.name + "." + member.Name + ": A parent of object referenced by field must have " + attribute.ComponentType + ", but none do."));
            }
        }

        fieldViolation = fieldViolationList.ToArray();
        if (fieldViolation.Length > 0)
        {
            return true;
        }
        return false;
    }

    private bool GameObjectHasParentWithComponent(GameObject gameObject, Type componentType)
    {
        while (gameObject.transform.parent != null)
        {
            gameObject = gameObject.transform.parent.gameObject;
            if (gameObject.GetComponent(componentType) != null)
            {
                return true;
            }
        }
        return false;
    }

    private GameObject GetGameObjectOfMember(MonoBehaviour target, MemberInfo member)
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

