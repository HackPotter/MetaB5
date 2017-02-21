using UnityEngine;

// TODO this is not done.
public class SetTimer : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private int _minutes;
    [SerializeField]
    private int _seconds;
#pragma warning restore 0067, 0649


    public override void OnEvent(ExecutionContext context)
    {
        GameContext.Instance.SetTimer(_minutes, _seconds);
    }
}

