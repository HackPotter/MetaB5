using UnityEngine;

public class ToolController : MonoBehaviour
{
    private ScanningFunction _scanningFunction;
    private ImpulseBeamGrabFunction _grabFunction;
    private LightFunction _lightFunction;

    void Start()
    {
        _scanningFunction = GetComponent<ScanningFunction>();
        _grabFunction = GetComponent<ImpulseBeamGrabFunction>();
        _lightFunction = GetComponent<LightFunction>();

        _scanningFunction.enabled = false;
        _grabFunction.enabled = false;
        _lightFunction.enabled = false;

        GameContext.Instance.Player.OnToolStateChanged += Player_OnToolStateChanged;
        GameContext.Instance.Player.OnLightStateChanged += Player_OnLightStateChanged;
    }

    void  Player_OnLightStateChanged(bool state)
    {
        _lightFunction.enabled = state;
    }

    void Player_OnToolStateChanged(ActiveTool activeTool)
    {
        switch (activeTool)
        {
            case ActiveTool.None:
                _grabFunction.enabled = false;
                _scanningFunction.enabled = false;
                break;
            case ActiveTool.ImpulseBeam:
                _scanningFunction.enabled = false;
                _grabFunction.enabled = true;
                break;
            case ActiveTool.Scanner:
                _grabFunction.enabled = false;
                _scanningFunction.enabled = true;
                break;
        }
    }

    void OnDestroy()
    {
        GameContext.Instance.Player.OnToolStateChanged -= Player_OnToolStateChanged;
        GameContext.Instance.Player.OnLightStateChanged -= Player_OnLightStateChanged;
    }

    void Update()
    {
        if (GameState.Instance.PauseLevel != PauseLevel.Unpaused)
        {
            GameContext.Instance.Player.LightEnabled = false;
            GameContext.Instance.Player.ActiveTool = ActiveTool.None;
            return;
        }

        if (Input.GetKeyDown(_scanningFunction.ShortcutKey))
        {
            ActiveTool currentTool = GameContext.Instance.Player.ActiveTool;
            GameContext.Instance.Player.ActiveTool = currentTool == ActiveTool.Scanner ? ActiveTool.None : ActiveTool.Scanner;
        }
        if (Input.GetKeyDown(_grabFunction.ShortcutKey))
        {
            ActiveTool currentTool = GameContext.Instance.Player.ActiveTool;
            GameContext.Instance.Player.ActiveTool = currentTool == ActiveTool.ImpulseBeam ? ActiveTool.None : ActiveTool.ImpulseBeam;
        }
        if (Input.GetKeyDown(_lightFunction.ShortcutKey))
        {
            if (!GameContext.Instance.Player.LightEnabled)
            {
                GameContext.Instance.Player.LightEnabled = GameContext.Instance.Player.ATP > 0.0f;
            }
            else
            {
                GameContext.Instance.Player.LightEnabled = false;
            }
        }
    }
}

