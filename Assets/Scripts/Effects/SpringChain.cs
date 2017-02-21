using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpringChain : MonoBehaviour
{
    public Transform AttachedTo;

    public float Springiness = 4.0f;

    public float MaxDistance = 1.25f;
    public float MinDistance = 0.75f;
    public bool LockPosition = false;

    void Awake()
    {

    }

    void FixedUpdate()
    {
        Vector3 targetPosition = AttachedTo.position;
        Vector3 direction = transform.position - targetPosition;
        if (LockPosition)
        {
            GetComponent<Rigidbody>().MovePosition(targetPosition + direction);
            return;
        }

        Debug.DrawLine(targetPosition, transform.position, Color.red);

        if (direction.magnitude > MaxDistance)
        {
            direction.Normalize();
            direction *= MaxDistance;
            GetComponent<Rigidbody>().MovePosition(targetPosition + direction);
        }
        else if (direction.magnitude < MinDistance)
        {
            direction.Normalize();
            direction *= MinDistance;
            GetComponent<Rigidbody>().MovePosition(targetPosition + direction);
        }

        Vector3 force = -direction * Springiness;
        GetComponent<Rigidbody>().AddForce(force);
        GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(direction.normalized));
    }
}
