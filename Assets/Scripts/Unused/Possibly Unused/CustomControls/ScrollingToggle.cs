// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class ScrollingToggle : MonoBehaviour
{
    private Rect rect;
    private Rect paddedRect;
    private string text;

    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    public bool selected;

    private bool Animating;
    private Vector2 scrollPosition;
    private Vector2 prevScrollPosition;

    private GUIStyle baseStyle;
    private GUIStyle labelStyle;
    private Texture2D NormalTexture;
    private Texture2D HoverTexture;
    private Texture2D OnNormalTexture;

    private int spacing;
    private const int scrollSpeed = 2;

    void Start()
    {
        Animating = false;
        scrollPosition = Vector2.zero;
        prevScrollPosition = Vector2.one;
    }

    void Update()
    {
        if (Animating)
        {
            if (scrollPosition != prevScrollPosition)
            {
                prevScrollPosition = scrollPosition;
                scrollPosition += Vector2.right * scrollSpeed;
            }
            else
            {
                scrollPosition = Vector2.zero;
            }
        }
    }

    public void init(string text, GUIStyle style)
    {
        this.text = text;
        this.selected = false;
        this.baseStyle = style;
        this.NormalTexture = this.baseStyle.normal.background;
        this.HoverTexture = this.baseStyle.hover.background;
        this.OnNormalTexture = this.baseStyle.onNormal.background;

        labelStyle = new GUIStyle(baseStyle);
        labelStyle.padding = new RectOffset(0, 0, 0, 0);
        labelStyle.normal.background = null;
        labelStyle.hover.background = null;
        labelStyle.active.background = null;
        labelStyle.focused.background = null;
        labelStyle.onNormal.background = null;
        labelStyle.onHover.background = null;
        labelStyle.onActive.background = null;
        labelStyle.onFocused.background = null;
    }

    private Rect transformRect(Rect rect, Matrix4x4 matrix)
    {
        int XPos;
        int YPos;
        int width;
        int height;

        Vector3 point;

        // Top left
        point = matrix.MultiplyPoint3x4(new Vector3(rect.x, rect.y, 1));
        XPos = (int)point.x;
        YPos = (int)point.y;

        // Bottom right
        point = matrix.MultiplyPoint3x4(new Vector3(rect.x + rect.width, rect.y + rect.height, 1));
        width = (int)(point.x - XPos);
        height = (int)(point.y - YPos);

        return new Rect(XPos, YPos, width, height);
    }

    private bool checkMousePosition()
    {
        Rect scaledRect = transformRect(rect, GUI.matrix);

        if (scaledRect.Contains(Event.current.mousePosition))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool draw(Rect newRect, bool newSelected, GUIStyle style)
    {
        selected = newSelected;

        if (rect != newRect)
        {
            rect = newRect;
            paddedRect = new Rect(rect.x + baseStyle.padding.left, rect.y + baseStyle.padding.top, rect.width - baseStyle.padding.right, rect.height - baseStyle.padding.bottom);
            spacing = (int)paddedRect.width;
        }

        if (rect.Contains(Event.current.mousePosition))
        {
            if (!Animating)
            {
                scrollPosition = new Vector2(spacing, 0);
                Animating = true;
            }
        }
        else
        {
            scrollPosition = new Vector2(spacing, 0);
            Animating = false;
        }

        //draw button
        if (selected)
            GUI.DrawTexture(rect, OnNormalTexture);
        else if (Animating)
            GUI.DrawTexture(rect, HoverTexture);
        else
            GUI.DrawTexture(rect, NormalTexture);

        if (GUI.Button(rect, "", "NoTexture"))
        {
            //callback
            selected = !selected;
        }

        GUILayout.BeginArea(paddedRect);
        
        GUILayout.BeginScrollView(scrollPosition, "NoTexture", "NoTexture", GUILayout.MinWidth(paddedRect.width), GUILayout.ExpandHeight(false), GUILayout.MinHeight(paddedRect.height), GUILayout.MaxHeight(paddedRect.height), GUILayout.Height(paddedRect.height));
        GUILayout.BeginHorizontal();
        GUILayout.Space(spacing);
        GUILayout.Label(text, style, GUILayout.Height(paddedRect.height));
        GUILayout.Space(spacing);
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        return selected;
    }
}