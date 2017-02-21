using System.Collections.Generic;

public delegate void ToolAdded(ITool tool);
public delegate void ToolRemoved(ITool tool);

public interface IEquippedTools
{
    event ToolAdded OnToolAdded;
    event ToolRemoved OnToolRemoved;

    void AddTool(ITool tool);
    List<ITool> Tools { get; }
}

