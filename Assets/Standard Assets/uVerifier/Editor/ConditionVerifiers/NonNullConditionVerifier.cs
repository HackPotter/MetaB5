using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;

/// <summary>
/// IConditionVerifier for the condition that a given GameObject field or MonoBehaviour field is non-null.
/// </summary>
public class NonNullConditionVerifier : BaseConditionVerifier<NonNullAttribute>
{
    protected override bool VerifyMember(MonoBehaviour target, MemberInfo member, NonNullAttribute[] attributes, out FieldViolation[] fieldViolation)
    {
        fieldViolation = null;
        if (member is FieldInfo)
        {
            object value = (member as FieldInfo).GetValue(target);
            if (VerifierUtils.IsNull(value))
            {
                fieldViolation = new FieldViolation[] {new FieldViolation(target.gameObject, null, target.name + "." + member.Name + ": Object referenced by field cannot be null.") };
                return true;
            }
        }

        return false;
    }
}

