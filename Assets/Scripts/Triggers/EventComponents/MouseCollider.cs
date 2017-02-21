using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public delegate void MouseEnterInteractable(MouseCollider target);
public delegate void MouseLeaveInteractable(MouseCollider target);
public delegate void MouseClickInteractable(MouseCollider target);

public class MouseCollider : MonoBehaviour
{
    public event MouseEnterInteractable MouseEntered;
    public event MouseLeaveInteractable MouseLeft;
    public event MouseClickInteractable MouseClicked;

    void OnMouseEnter()
    {
        if (MouseEntered != null)
        {
            MouseEntered(this);
        }
    }

    void OnMouseExit()
    {
        if (MouseLeft != null)
        {
            MouseLeft(this);
        }
    }

    void OnMouseUpAsButton()
    {
        if (MouseClicked != null)
        {
            MouseClicked(this);
        }
    }
}
