using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squid;
using UnityEngine;
using System.Collections;

[Toolbox("Metablast")]
public class RadialSelector : Control
{
    private int _selected;
    private int _itemCount;
    private float _radius;
    private string _texture;

    private float _currentAngle;

    private Control[] _items;

    [Texture]
    public string ItemTexture
    {
        get { return _texture; }
        set
        {
            _texture = value;
            InitializeItems();
        }
    }

    public int Selected
    {
        get { return _selected; }
        set
        {
            _selected = value;
        }
    }

    public int ItemCount
    {
        get { return _itemCount; }
        set
        {
            _itemCount = value;
            InitializeItems();
        }
    }

    public float Radius
    {
        get { return _radius; }
        set
        {
            _radius = value;
            InitializeItems();
        }
    }

    public Point SelectedSize
    {
        get;
        set;
    }

    public Point UnselectedSize
    {
        get;
        set;
    }

    public RadialSelector()
    {
        Animation.Custom(SpinAnimation());
    }

    private IEnumerator SpinAnimation()
    {
        while (true)
        {
            if (ItemCount == 0)
            {
                yield return null;
                continue;
            }
            _selected = Mathf.Clamp(_selected, 0, ItemCount - 1);

            float angleOffset = CalculateTargetAngle(_selected);
            _currentAngle = Mathf.LerpAngle(_currentAngle * Mathf.Rad2Deg, angleOffset * Mathf.Rad2Deg, 0.15f) * Mathf.Deg2Rad;
            for (int i = 0; i < _itemCount; i++)
            {
                Control control = _items[i];
                float defaultAngle = CalculateTargetAngle(i);
                float targetAngle = defaultAngle - _currentAngle - Mathf.PI / 2;

                int xPos = (int)(Radius * Mathf.Cos(targetAngle));
                int yPos = (int)(Radius * Mathf.Sin(targetAngle));
                control.Size = UnselectedSize;
                control.Position = new Point(xPos, yPos) + Size / 2 - control.Size / 2;
                
            }
            _items[_selected].Position += _items[_selected].Size / 2;
            _items[_selected].Size = SelectedSize;
            _items[_selected].Position -= _items[_selected].Size / 2;
            yield return null;
        }
    }

    protected override void OnLateUpdate()
    {
        base.OnLateUpdate();
    }

    private void UpdateSelection()
    {
        _selected = Mathf.Clamp(_selected, 0, _itemCount - 1);
    }

    private float CalculateTargetAngle(int index)
    {
        return (float)(index) / ItemCount * Mathf.PI * 2;
    }

    private void InitializeItems()
    {
        _itemCount = Mathf.Max(_itemCount, 0);
        if (_items != null)
        {
            foreach (var control in _items)
            {
                Elements.Remove(control);
            }
        }
        _currentAngle = CalculateTargetAngle(0);
        _items = new Button[ItemCount];

        for (int i = 0; i < ItemCount; i++)
        {
            //float radians = CalculateTargetAngle(i);

            Button control = new Button();
            control.Text = "" + i;

            //int xPos = (int)(Radius * Mathf.Cos(radians));
            //int yPos = (int)(Radius * Mathf.Sin(radians));
            //control.Position = new Point(xPos, yPos) + Size / 2;
            _items[i] = control;
            Elements.Add(control);
        }
    }
}

