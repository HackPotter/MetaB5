using UnityEngine;

[Trigger(Description = "DEPRECATED: Use EnableGameObject instead.", DisplayPath = "Deprecated")]
[AddComponentMenu("Metablast/Triggers/Actions/GameObjects/Deprecated Enable Game Object")]
public class DeprecatedEnableGameObjectAction : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private GameObject _gameObject;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        if (_gameObject)
        {
            _gameObject.SetActive(true);
        }
    }
}
