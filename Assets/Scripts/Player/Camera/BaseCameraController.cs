using UnityEngine;

[RequireComponent(typeof(ControlServices))]
public abstract class BaseCameraController : MonoBehaviour, ICameraController
{
    public abstract void OnAcquiredControl();

    public abstract void OnLostControl();


}