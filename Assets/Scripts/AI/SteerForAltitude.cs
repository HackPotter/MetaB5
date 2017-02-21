using UnityEngine;

public class SteerForAltitude : Steering
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private float _minimumAltitude = 24.5f;
    [SerializeField]
    private float _maximumAltitude = 45f;
#pragma warning restore 0067, 0649

    public override Vector3 GetAcceleration()
    {
        if (Agent.transform.position.y < _minimumAltitude)
        {
            return Vector3.up;
        }
        else if (Agent.transform.position.y > _maximumAltitude)
        {
            return Vector3.down;
        }

        return Vector3.zero;
    }
}