using UnityEngine;

public class WaterDropletDeformer : MonoBehaviour
{
    public float xPeriod;
    public float yPeriod;
    public float zPeriod;

    public float xOffset;
    public float yOffset;
    public float zOffset;

    public float xMagnitude;
    public float yMagnitude;
    public float zMagnitude;

    public Rigidbody _rigidbody;
    public float vDeformMagnitude;

    private Vector3 _lastVelocity;

    void FixedUpdate()
    {
        Vector3 acceleration = _rigidbody.velocity - _lastVelocity;

        float xScale = 1 + acceleration.x / (acceleration.x + vDeformMagnitude);
        float yScale = 1 + acceleration.y / (acceleration.y + vDeformMagnitude);
        float zScale = 1 + acceleration.z / (acceleration.z + vDeformMagnitude);

        transform.localScale = new Vector3(xScale, yScale, zScale);

        _lastVelocity = _rigidbody.velocity;
    }
}
