using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum AxisRotatorAxis
{
    X,
    Y,
    Z
}
public class AxisRotator : MonoBehaviour
{
    public AxisRotatorAxis Axis;
    public float Acceleration = 10f;
    public float Decay = 0.95f;
    public float MaxSpeed = 10f;


    private float _speed;

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _speed += Acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _speed += Acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            _speed += Acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _speed -= Acceleration * Time.deltaTime;
        }

        _speed = Mathf.Clamp(_speed, -MaxSpeed * Time.deltaTime, MaxSpeed * Time.deltaTime);
        _speed *= Decay;

        switch (Axis)
        {
            case AxisRotatorAxis.X:
                transform.localRotation *= Quaternion.AngleAxis(_speed, Vector3.right);
                break;
            case AxisRotatorAxis.Y:
                transform.localRotation *= Quaternion.AngleAxis(_speed, Vector3.up);
                break;
            case AxisRotatorAxis.Z:
                transform.localRotation *= Quaternion.AngleAxis(_speed, Vector3.forward);
                break;
        }
    }
}

