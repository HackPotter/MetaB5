using UnityEngine;

public enum InitializationMethod
{
    Awake = 0,
    Start = 1,
    OnEnable = 2
}

public class SetPositionOnStart : MonoBehaviour
{
    [SerializeField]
    private InitializationMethod _whenToSet;

    [SerializeField]
    private Transform _targetPosition;
    
    void OnEnable()
    {
        if (_whenToSet == InitializationMethod.OnEnable)
        {
            SetPosition();
        }
    }

    void Awake()
    {
        if (_whenToSet == InitializationMethod.Awake)
        {
            SetPosition();
        }
    }

    void Start()
    {
        if (_whenToSet == InitializationMethod.Start)
        {
            SetPosition();
        }
    }

    private void SetPosition()
    {
        Debug.Log("SetPositionOnStart:Setting Position");
        transform.position = _targetPosition.position;
        transform.rotation = _targetPosition.rotation;
    }
}
