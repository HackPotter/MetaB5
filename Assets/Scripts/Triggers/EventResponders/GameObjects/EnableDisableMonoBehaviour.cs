using UnityEngine;

[Trigger(Description = "Enables or disables the given MonoBehaviour.")]
public class EnableDisableMonoBehaviour : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private bool _enable;
    [SerializeField]
    private MonoBehaviour _monoBehaviour;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        _monoBehaviour.enabled = _enable;
    }
}

