using Squid;
using UnityEngine;
using System.Collections;

public class ContextMessageView : IContextMessageView
{
    private Frame _contextMessageFrame;

    private TextArea _contextMessageTextArea;
    private ImageControl _contextMessageBackground;

    private int _defaultBackgroundColor;
    private int _targetBackgroundColor;
    private int _currentBackgroundColor;

    private string _lowPriorityText = "";
    private string _text = "";

    public void SetLowPriorityText(string text)
    {
        _lowPriorityText = text;
        SetText();
    }

    public void SetText(string text)
    {
        _text = text;
        _currentBackgroundColor = _defaultBackgroundColor;

        SetText();
    }

    public void SetText(string text, Color color)
    {
        _text = text;
        _currentBackgroundColor = ColorInt.ARGB(color.a, color.r, color.g, color.b);
        SetText();
    }

    private void SetText()
    {
        if (!string.IsNullOrEmpty(_text))
        {
            _contextMessageTextArea.Text = _text;
            _targetBackgroundColor = _currentBackgroundColor;
            _contextMessageFrame.Visible = true;
        }
        else if (!string.IsNullOrEmpty(_lowPriorityText))
        {
            _contextMessageTextArea.Text = _lowPriorityText;
            _targetBackgroundColor = _defaultBackgroundColor;
            _contextMessageFrame.Visible = true;
        }
        else
        {
            _contextMessageFrame.Visible = false;
            _contextMessageTextArea.Text = "";
        }
    }

    public ContextMessageView(Frame contextMessageFrame)
    {
        _contextMessageFrame = contextMessageFrame;
        _contextMessageBackground = (ImageControl)contextMessageFrame.GetControl("ContextMessageBackground");
        _contextMessageTextArea = (TextArea)contextMessageFrame.GetControl("ContextMessageLabel");
        _contextMessageTextArea.UseTextColor = false;

        _defaultBackgroundColor = _contextMessageBackground.Color;

        _contextMessageFrame.Animation.Custom(ColorFadeAnimation());
    }

    IEnumerator ColorFadeAnimation()
    {
        while (true)
        {
            _contextMessageBackground.Color = ColorInt.Blend(_contextMessageBackground.Color, _targetBackgroundColor, 0.80f);
            yield return null;
        }
    }
}

