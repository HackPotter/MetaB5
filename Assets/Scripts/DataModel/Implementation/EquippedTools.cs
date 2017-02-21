using System.Collections.Generic;

public class EquippedTools : IEquippedTools
{
    private List<ITool> _tools = new List<ITool>();

    public List<ITool> Tools
    {
        get { return _tools; }
    }

    public event ToolAdded OnToolAdded;
    public event ToolRemoved OnToolRemoved;

    public void AddTool(ITool tool)
    {
        _tools.Add(tool);
        if (OnToolAdded != null)
        {
            OnToolAdded(tool);
        }
    }

    public void RemoveTool(ITool tool)
    {
        _tools.Remove(tool);
        if (OnToolRemoved != null)
        {
            OnToolRemoved(tool);
        }
    }
}

