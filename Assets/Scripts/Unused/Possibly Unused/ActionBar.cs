using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squid;

[Toolbox("Metablast")]
public class ActionBar : Control
{
    private IEquippedTools _tools;
    private Frame _toolButtonFrame;

    public IEquippedTools Tools
    {
        get
        {
            return _tools;
        }
        set
        {
            RegisterTools(_tools);
        }
    }

    public ActionBar()
    {
        _toolButtonFrame = new Frame();
        _toolButtonFrame.Dock = DockStyle.Fill;
        _toolButtonFrame.Name = "ToolButtonFrame";
        _toolButtonFrame.NoEvents = true;

        Elements.Add(_toolButtonFrame);
    }

    private void RegisterTools(IEquippedTools tools)
    {
        _tools = tools;
        _tools.OnToolAdded += new ToolAdded(_tools_OnToolAdded);
        _tools.OnToolRemoved += new ToolRemoved(_tools_OnToolRemoved);

        foreach (var tool in Tools.Tools)
        {
            ActionBarItemView toolView = new ActionBarItemView(new ActionBarItem(tool));
            _toolButtonFrame.Controls.Add(toolView);
        }
    }

    void _tools_OnToolAdded(ITool tool)
    {
    }

    void _tools_OnToolRemoved(ITool tool)
    {
    }
}

