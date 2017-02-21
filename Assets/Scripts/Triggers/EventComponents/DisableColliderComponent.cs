using UnityEngine;

public delegate void ColliderEnabledEventHandler(DisableColliderComponent sender, Collider collider);

[AddComponentMenu("Metablast/Triggers/Components/Disable Collider Component")]
public class DisableColliderComponent : MonoBehaviour
{
    public event ColliderEnabledEventHandler ColliderEnabled;
    public event ColliderEnabledEventHandler ColliderDisabled;
#pragma warning disable 0067, 0649
    [SerializeField]
    private Collider _componentToDisable;
#pragma warning restore 0067, 0649

    void Awake()
    {
        if (!_componentToDisable)
        {
            DebugFormatter.LogError(this, "Component To Disable is null.");
            Destroy(this);
        }
    }

    public void EnableComponent()
    {
        _componentToDisable.enabled = true;
        if (ColliderEnabled != null)
        {
            ColliderEnabled(this, _componentToDisable);
        }
    }

    public void DisableComponent()
    {
        _componentToDisable.enabled = false;
        if (ColliderDisabled != null)
        {
            ColliderDisabled(this, _componentToDisable);
        }
    }
}

