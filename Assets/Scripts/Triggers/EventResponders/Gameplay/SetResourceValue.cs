using UnityEngine;

public enum ResourceType
{
    ATP,
    NADPH,
    O2
}

public enum ResourceChangeMode
{
    Add,
    Subtract,
    Set
}

public class SetResourceValue : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private float _value;
    [SerializeField]
    private ResourceType _resource;
    [SerializeField]
    private ResourceChangeMode _resourceChangeMode;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        switch (_resource)
        {
            case ResourceType.ATP:
                SetATP();
                break;
            case ResourceType.NADPH:
                SetNADPH();
                break;
            case ResourceType.O2:
                SetO2();
                break;
        }
    }

    private void SetATP()
    {
        switch (_resourceChangeMode)
        {
            case ResourceChangeMode.Add:
                GameContext.Instance.Player.ATP += _value;
                break;
            case ResourceChangeMode.Set:
                GameContext.Instance.Player.ATP = _value;
                break;
            case ResourceChangeMode.Subtract:
                GameContext.Instance.Player.ATP -= _value;
                break;
        }
    }

    private void SetNADPH()
    {
        switch (_resourceChangeMode)
        {
            case ResourceChangeMode.Add:
                GameContext.Instance.Player.NADPH += _value;
                break;
            case ResourceChangeMode.Set:
                GameContext.Instance.Player.NADPH = _value;
                break;
            case ResourceChangeMode.Subtract:
                GameContext.Instance.Player.NADPH -= _value;
                break;
        }
    }
    private void SetO2()
    {
        switch (_resourceChangeMode)
        {
            case ResourceChangeMode.Add:
                GameContext.Instance.Player.O2 += _value;
                break;
            case ResourceChangeMode.Set:
                GameContext.Instance.Player.O2 = _value;
                break;
            case ResourceChangeMode.Subtract:
                GameContext.Instance.Player.O2 -= _value;
                break;
        }
    }
}

