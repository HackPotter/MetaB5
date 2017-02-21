using UnityEngine;

[Trigger(DisplayPath = "Debug")]
[AddComponentMenu("Metablast/Triggers/Actions/Debug/Print Console Message")]
public class PrintConsoleMessage : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _message;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        DebugFormatter.Log(this, _message);
    }
}
