using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour
{
    void Awake()
    {
        this.gameObject.layer = LayerMask.NameToLayer("GrabbableObject");
    }
}

