using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// IConditionVerifier for the condition that a given GameObject field or MonoBehaviour field contains a component of the type specified in the HasComponentAttribute.
/// </summary>
public class HasComponentVerifier : BaseConditionVerifier<HasComponentAttribute>
{
    protected override bool VerifyMember(MonoBehaviour target, MemberInfo member, HasComponentAttribute[] attributes, out FieldViolation[] fieldViolation)
    {
        List<FieldViolation> fieldViolationsList = new List<FieldViolation>();

        if (!(member is FieldInfo))
        {
            fieldViolation = null;
            return false;
        }

        object memberValue = (member as FieldInfo).GetValue(target);
        if (memberValue is GameObject)
        {
            GameObject gameObject = memberValue as GameObject;
            foreach (HasComponentAttribute attribute in attributes)
            {
                Type requiredComponentType = attribute.ComponentType;
                if (VerifierUtils.IsNull(gameObject))
                {
                    fieldViolationsList.Add(new FieldViolation(target.gameObject, null, target.name + "." + member.Name + ": Object referenced by field must have " + requiredComponentType + ", but is null."));
                    continue;
                }
                if (gameObject.GetComponent(requiredComponentType) == null)
                {
                    fieldViolationsList.Add(new FieldViolation(target.gameObject, gameObject, target.name + "." + member.Name + ": Object referenced by field must have " + requiredComponentType + ", but does not."));
                }
            }
        }
        else if (memberValue is MonoBehaviour)
        {
            MonoBehaviour component = memberValue as MonoBehaviour;
            foreach (HasComponentAttribute attribute in attributes)
            {
                Type requiredComponentType = attribute.ComponentType;
                if (VerifierUtils.IsNull(component))
                {
                    fieldViolationsList.Add(new FieldViolation(target.gameObject, null, target.name + "." + member.Name + ": Object referenced by field must have " + requiredComponentType + ", but is null."));
                    continue;
                }
                if (component.GetComponent(requiredComponentType) == null)
                {
                    fieldViolationsList.Add(new FieldViolation(target.gameObject, component, target.name + "." + member.Name + ": Object referenced by field must have " + requiredComponentType + ", but does not."));
                }
            }

        }

        fieldViolation = fieldViolationsList.ToArray();

        if (fieldViolation.Length > 0)
        {
            return true;
        }
        return false;
    }
}
