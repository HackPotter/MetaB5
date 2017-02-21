using System;
using UnityEngine;

public delegate void ObjectiveTaskCompleted(ObjectiveTask task);

[Serializable]
public class ObjectiveTask
{
#pragma warning disable 0649
    [SerializeField]
    [Infobox("The name of the task.")]
    private string _name;

    [SerializeField]
    [Infobox("A description of the task [UNUSED].")]
    private string _description;
#pragma warning restore 0649

    private bool _completed;

    public event ObjectiveTaskCompleted TaskCompleted;

    public bool IsComplete
    {
        get { return _completed; }
    }

    public void Complete()
    {
        _completed = true;
        if (TaskCompleted != null)
        {
            TaskCompleted(this);
        }
    }

    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _description; } set { _description = value; } }
    public object Data { get; set; }
}
