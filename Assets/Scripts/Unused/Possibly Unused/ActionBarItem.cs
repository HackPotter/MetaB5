using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ActionBarItem
{
    private ITool _tool;

    public ITool Tool
    {
        get { return _tool; }
        set { _tool = value; }
    }

    public ActionBarItem(ITool tool)
    {
        _tool = tool;
    }
}

