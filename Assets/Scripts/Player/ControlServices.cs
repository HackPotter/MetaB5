using UnityEngine;

public class ControlServices : MonoBehaviour
{
    public static ControlServices Instance
    {
        get;
        private set;
    }

#pragma warning disable 0067, 0649
    [SerializeField]
    private CameraControllerInterface _camera;
#pragma warning restore 0067, 0649

    public CameraControllerInterface ControlledCamera
    {
        get { return _camera; }
    }


    void Awake()
    {
        Instance = this;
    }
}

