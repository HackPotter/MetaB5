// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

public class MenuButton : UnityEngine.Object
{
    protected Vector2 _drawPosition;
    protected Vector2 _drawDimensions;

    protected Vector2 _position;
    protected Vector2 _dimensions;
    protected string _text;

    protected Action _clickEvent;

    protected GUIStyle _Style;

    public MenuButton() { }

    public MenuButton(Vector2 position, Vector2 dimensions, Vector2 drawPosition, string text, Action clickEvent)
    {
        _drawPosition = drawPosition;
        _position = position;
        _drawDimensions = _dimensions = dimensions;
        _text = text;
        _clickEvent = clickEvent;
        _Style = new GUIStyle();
        _Style.normal.textColor = Color.white;
        _Style.hover.textColor = new Color(7 / 255, 37 / 255, 57 / 255, 1);
        _Style.fontSize = 32;
        _Style.alignment = TextAnchor.MiddleCenter;
        _Style.overflow.left = 5;
        _Style.overflow.right = 5;
        _Style.font = (Font)Resources.Load("Assets/Styling/Fonts/orbitron-light.ttf", typeof(Font));
        _Style.normal.background = (Texture2D)Resources.Load("Assets/Textures/UI/Menu/Shared/Buttons/Button_Normal.png", typeof(Texture2D));
        _Style.hover.background = (Texture2D)Resources.Load("Assets/Textures/UI/Menu/Shared/Buttons/Button_Active.png", typeof(Texture2D));
    }

    public virtual void draw()
    {
        if (GUI.Button(new Rect(_drawPosition.x, _drawPosition.y, _drawDimensions.x, _drawDimensions.y), _text))
        {
            _clickEvent();
        }
    }

    public void drawLevelButtons()
    {
        if (GUI.Button(new Rect(_drawPosition.x, _drawPosition.y, _drawDimensions.x, _drawDimensions.y), _text, _Style))
        {
            _clickEvent();
        }
    }

    /*public void  draw ( GUIStyle style  ){
        //GUI.skin.button.normal.background = texture;
        if(GUI.Button( new Rect(_drawPosition.x, _drawPosition.y, _drawDimensions.x, _drawDimensions.y),_text, style))
        {
            _clickEvent();
        }
    }*/

    public Rect getRect()
    {
        return new Rect(_drawPosition.x, _drawPosition.y, _drawDimensions.x, _drawDimensions.y);
    }

    public Rect getTransformedRect(Matrix4x4 matrix)
    {
        int XPos;
        int YPos;
        int width;
        int height;

        Vector3 point;

        // Top left
        point = matrix.MultiplyPoint3x4(new Vector3(_drawPosition.x, _drawPosition.y, 1));
        XPos = (int)point.x;
        YPos = (int)point.y;

        // Bottom right
        point = matrix.MultiplyPoint3x4(new Vector3(_drawPosition.x + _drawDimensions.x, _drawPosition.y + _drawDimensions.y, 1));
        width = (int)(point.x - XPos);
        height = (int)(point.y - YPos);

        return new Rect(XPos, YPos, width, height);
    }

    public void setDrawPosition(Vector2 input)
    {
        _drawPosition = input;
    }

    public void setDrawDimensions(Vector2 input)
    {
        _drawDimensions = input;
    }

    public void setNormalTexture(Texture2D texture, Color color)
    {
        _Style.normal.background = texture;
        _Style.normal.textColor = color;
    }

    public Vector2 getPosition() { return _position; }

}