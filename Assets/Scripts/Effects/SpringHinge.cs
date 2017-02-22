#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpringHinge : MonoBehaviour
{
    public GameObject _nextLink;
    public float _springConstant;

    private Rigidbody _childRigidbody;
    private Vector3 _targetOffset;
    private Quaternion _targetRotationOffset;

    void Awake()
    {
        if (_nextLink == null)
        {
            DebugFormatter.LogError(this, "Child link must not be null.");
            return;
        }
        _childRigidbody = _nextLink.GetComponent<Rigidbody>();
        if (_childRigidbody == null)
        {
            DebugFormatter.LogError(this, "Child link must have rigidbody.");
            return;
        }

        _targetOffset = _childRigidbody.transform.position - transform.position;
        _targetRotationOffset = Quaternion.FromToRotation(transform.forward, _childRigidbody.transform.forward);
    }
}

