using UnityEngine;

[ChildHasComponent(typeof(WheelCollider))]
[ParentHasComponent(typeof(Rigidbody))]
public class TestScript : MonoBehaviour
{
    
    [NonNull]
    [HasComponent(typeof(BoxCollider))]
    [ChildHasComponent(typeof(WheelCollider))]
    [ParentHasComponent(typeof(Rigidbody))]
    public GameObject SomeRigidBody;
    
}

