using UnityEngine;

[Trigger(Description= "DEPRECATED: Use DisableGameObject instead.", DisplayPath = "Deprecated")]
[AddComponentMenu("Metablast/Triggers/Actions/GameObjects/Disable Game Object")]
public class DeprecatedDisableGameObjectAction : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private GameObject _gameObject;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        if (_gameObject)
        {
            _gameObject.SetActive(false);
        }
    }
}
