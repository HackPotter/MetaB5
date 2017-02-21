using UnityEngine;
using UnityEngine.EventSystems;

public enum MouseButton
{
    Left = 0,
    Right = 1,
    Middle = 2
}

public enum MouseClickTargets
{
    Everything,
    IgnoreUserInterface,
}

public enum MouseClickType
{
    MouseDown,
    MouseUp,
    MouseHold,
}

[Trigger(Description = @"Invoked when the player has clicked.
If the MouseClickType is MouseDown, it will be invoked the first frame the player has clicked.
If the MouseClickType is MouseUp, it will be invoked the first frame the player has released the mouse button.
If the MouseClickType is MouseHold, it will be invoked every frame while the mouse button is held.
Specify whether the user interface will be included with MouseTargets.", DisplayPath = "Input")]
public class MouseClick : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The mouse button for which this event will be invoked when pressed, released, or every frame it is held.")]
    private MouseButton _mouseButton;
    [SerializeField]
    [Infobox("Whether this event will be invoked when clicking UI or not.")]
    private MouseClickTargets _mouseTargets;
    [SerializeField]
    [Infobox("Whether this event will be invoked on press, release, or hold.")]
    private MouseClickType _mouseClickType;
#pragma warning restore 0067, 0649

    void Update()
    {
        switch (_mouseClickType)
        {
            case MouseClickType.MouseDown:
                if (Input.GetMouseButtonDown((int)_mouseButton) && (!EventSystem.current.IsPointerOverGameObject() || _mouseTargets == MouseClickTargets.Everything))
                {
                    TriggerEvent();
                }
                break;
            case MouseClickType.MouseUp:
                if (Input.GetMouseButtonUp((int)_mouseButton) && (!EventSystem.current.IsPointerOverGameObject() || _mouseTargets == MouseClickTargets.Everything))
                {
                    TriggerEvent();
                }
                break;
            case MouseClickType.MouseHold:
                if (Input.GetMouseButton((int)_mouseButton) && (!EventSystem.current.IsPointerOverGameObject() || _mouseTargets == MouseClickTargets.Everything))
                {
                    TriggerEvent();
                }
                break;
        }
    }
}

