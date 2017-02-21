using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;

/// <summary>
/// A base class for IConditionVerifiers implementors to extend.
/// </summary>
/// <typeparam name="T">The type of ConditionVerifierAttribute that the extending IConditionVerifier uses to verify conditions.</typeparam>
public abstract class BaseConditionVerifier<T> : IConditionVerifier where T : ConditionVerifierAttribute
{
    /// <summary>
    /// Returns the Type of the ConditionVerifierAttribute this BaseConditionVerifier uses.
    /// </summary>
    protected Type GetAttributeType
    {
        get { return typeof(T); }
    }

    /// <summary>
    /// Verifies that a member does not violate the condition enforced by the extender of this class.
    /// </summary>
    /// <param name="target">The MonoBehaviour on which to enforce conditions (guaranteed non-null).</param>
    /// <param name="member">The MemberInfo of the member of the target being verified (guaranteed non-null).</param>
    /// <param name="attributes">The ConditionVerifierAttributes of type T found on the MemberInfo (guaranteed non-null and non-empty).</param>
    /// <param name="fieldViolation">A List of FieldViolations representing violations of the enforced condition (must not be null).</param>
    /// <returns>True if violations were found, otherwise false.</returns>
    protected abstract bool VerifyMember(MonoBehaviour target, MemberInfo member, T[] attributes, out FieldViolation[] fieldViolation);

    /// <summary>
    /// Verifies a condition on the specified GameObject.
    /// </summary>
    /// <param name="gameObject">The GameObject on which to enforce a condition.</param>
    /// <returns>A List of FieldViolations that are present, or an empty list of no such violations were found.</returns>
    public List<FieldViolation> VerifyCondition(GameObject gameObject)
    {
        Asserter.NotNull(gameObject, "BaseConditionVerifier.VerifyConditions:gameObject is null");

        List<FieldViolation> fieldViolations = new List<FieldViolation>();
        foreach (MonoBehaviour component in gameObject.GetComponents<MonoBehaviour>())
        {
            if (!component)
            {
                continue;
            }
            List<MemberInfo> members = VerifierUtils.GetMembersWithAttribute(component, GetAttributeType);

            foreach (MemberInfo member in members)
            {
                FieldViolation[] fieldViolation;
                if (VerifyMember(component, member,VerifierUtils.GetAttributesOfType<T>(member), out fieldViolation))
                {
                    fieldViolations.AddRange(fieldViolation);
                }
            }
        }

        return fieldViolations;
    }
}
