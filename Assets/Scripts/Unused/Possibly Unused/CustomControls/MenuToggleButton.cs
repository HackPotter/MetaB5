// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System;

class MenuToggleButton : MenuButton
{
    private bool _selected;
    private Func<bool> _eval;

    public MenuToggleButton(Vector2 position, Vector2 dimensions, Vector2 drawPosition, string text, Func<bool> evalString, Action clickEvent)
        : base(position, dimensions, drawPosition, text, clickEvent)
    {
        _drawPosition = drawPosition;
        _position = position;
        _drawDimensions = _dimensions = dimensions;
        _text = text;
        _clickEvent = clickEvent;
        _eval = evalString;
        _selected = evalString();
    }

    public override void draw()
    {
        _selected = _eval();
        if (GUI.Toggle(new Rect(_drawPosition.x, _drawPosition.y, _drawDimensions.x, _drawDimensions.y), _selected, _text))
        {
            _clickEvent();
        }
    }
}