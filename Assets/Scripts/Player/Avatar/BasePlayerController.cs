using UnityEngine;

[RequireComponent(typeof(ControlServices))]
public abstract class BasePlayerController : MonoBehaviour, IPlayerController
{
    public abstract void OnAcquiredControl();

    public abstract void OnLostControl();
}

