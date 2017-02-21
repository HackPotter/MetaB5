using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void GameplayObjectiveCompleted(GameplayObjective objective);
public delegate void GameplayObjectiveTaskAdded(GameplayObjective objective, ObjectiveTask task);
public delegate void GameplayObjectiveTaskCompleted(GameplayObjective objective, ObjectiveTask task);

[Serializable]
public class GameplayObjective
{
    public event GameplayObjectiveCompleted Completed;
    public event GameplayObjectiveTaskAdded TaskAdded;
    public event GameplayObjectiveTaskCompleted TaskCompleted;

#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The tasks of this objective.")]
    private List<ObjectiveTask> _tasks = new List<ObjectiveTask>();

    [SerializeField]
    [Infobox("The name of the objective.")]
    private string _name;

    [SerializeField]
    [Infobox("A description of the objective [UNUSED].")]
    private string _description;
#pragma warning restore 0067, 0649

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public List<ObjectiveTask> Tasks
    {
        get { return _tasks; }
    }

    public bool IsComplete
    {
        get
        {
            return _tasks.TrueForAll((t) => t.IsComplete);
        }
    }

    public GameplayObjective()
    {
    }

    private bool _initialized = false;
    public void Initialize()
    {
        if (!_initialized)
        {
            foreach (ObjectiveTask task in Tasks)
            {
                task.TaskCompleted += new ObjectiveTaskCompleted(task_TaskCompleted);
            }
        }
    }

    void task_TaskCompleted(ObjectiveTask task)
    {
        if (TaskCompleted != null)
        {
            TaskCompleted(this, task);
        }

        if (IsComplete)
        {
            if (Completed != null)
            {
                Completed(this);
            }
        }
    }

    public void AddTask(ObjectiveTask task)
    {
        Tasks.Add(task);
        task.TaskCompleted += new ObjectiveTaskCompleted(task_TaskCompleted);
        if (TaskAdded != null)
        {
            TaskAdded(this, task);
        }
    }

    public void Complete()
    {
        foreach (var task in _tasks)
        {
            task.Complete();
        }
    }
}

