using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class TriggerEditorNode : IEnumerable<TriggerEditorNode>
{
    private List<TriggerEditorNode> _children = new List<TriggerEditorNode>();
    private bool _expanded = true;
    private bool _delete = false;
    private EventEditorContext _context;

    public EventEditorContext Context
    {
        get { return _context; }
        set
        {
            _context = value;
            foreach (var child in this)
            {
                child.Context = value;
            }
        }
    }

    public TriggerEditorNode Parent
    {
        get;
        private set;
    }

    public int ExecutionRank
    {
        get
        {
            if (TriggerComponent is IOrderable)
            {
                return (TriggerComponent as IOrderable).Ordinal;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (TriggerComponent is IOrderable)
            {
                (TriggerComponent as IOrderable).Ordinal = value;
            }
        }
    }

    public bool IsDeleted
    {
        get { return _delete; }
    }

    public bool Expanded
    {
        get { return _expanded; }
        set
        {

            if (_expanded && !value)
            {
                foreach (var child in this)
                {
                    child.Expanded = false;
                }
            }
            _expanded = value;
        }
    }

    public TriggerEditorNode(EventEditorContext context)
    {
        _context = context;
    }

    public void OnGUI(int depth)
    {
        if (Event.current.type == EventType.Layout)
        {
            _children.Sort((n1, n2) => { return n1.ExecutionRank - n2.ExecutionRank; });
        }
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(25 * depth);
        EditorGUILayout.BeginVertical();

        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
        GUILayout.Space(2);

        DrawGUI();

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();


        //if (Expanded)
        //{
        foreach (TriggerEditorNode node in this)
        {
            node.OnGUI(depth + 1);
        }
        //}
    }

    public abstract void DrawGUI();

    public abstract TriggerComponent TriggerComponent
    {
        get;
    }

    public abstract Dictionary<string, Variable> GetOutputVariables();

    public abstract Dictionary<string, Variable> GetScopeVariables();

    public void Add(TriggerEditorNode child)
    {
        child.Parent = this;
        _children.Add(child);
    }

    public void Delete()
    {
        foreach (TriggerEditorNode child in this)
        {
            child.Delete();
        }

        _delete = true;
        OnNodeDeleted();
    }

    protected abstract void OnNodeDeleted();

    public IEnumerator<TriggerEditorNode> GetEnumerator()
    {
        List<TriggerEditorNode> toDelete = new List<TriggerEditorNode>();
        foreach (TriggerEditorNode child in _children)
        {
            if (child.IsDeleted)
            {
                toDelete.Add(child);
            }
        }

        foreach (TriggerEditorNode child in toDelete)
        {
            _children.Remove(child);
        }

        _children.Sort((n1, n2) => { return n1.ExecutionRank - n2.ExecutionRank; });
        for (int i = 0; i < _children.Count; i++)
        {
            _children[i].ExecutionRank = i;
        }

        foreach (TriggerEditorNode child in _children)
        {
            yield return child;
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

