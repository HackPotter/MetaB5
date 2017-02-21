using System.Collections.Generic;
using System.Linq;

public class UserObjectives : IUserObjectives
{
    public event UserObjectiveAdded UserObjectiveAdded;
    public event UserObjectiveTaskAdded UserObjectiveTaskAdded;
    public event UserObjectiveRemoved UserObjectiveRemoved;
    public event UserObjectiveCompleted UserObjectiveCompleted;
    public event UserObjectiveTaskCompleted UserObjectiveTaskCompleted;

    private List<GameplayObjective> _activeObjectives = new List<GameplayObjective>();

    public UserObjectives()
    {
    }

    public List<GameplayObjective> ActiveObjectives
    {
        get { return _activeObjectives; }
    }

    public void AddObjective(GameplayObjective objective)
    {
		if (_activeObjectives.FindIndex((o) => o.Name == objective.Name) >= 0)
		{
			return;
		}
		objective.Initialize();
        objective.Completed += OnObjectiveCompleted;
        objective.TaskAdded += new GameplayObjectiveTaskAdded(objective_TaskAdded);
        objective.TaskCompleted += new GameplayObjectiveTaskCompleted(objective_TaskCompleted);
        _activeObjectives.Add(objective);
        AnalyticsLogger.Instance.AddLogEntry(new ObjectiveAddedLogEntry(GameContext.Instance.Player.UserGuid, objective));
        if (UserObjectiveAdded != null)
        {
            UserObjectiveAdded(objective);
        }
    }

    void objective_TaskCompleted(GameplayObjective objective, ObjectiveTask task)
    {
        AnalyticsLogger.Instance.AddLogEntry(new ObjectiveTaskCompleteLogEntry(GameContext.Instance.Player.UserGuid, objective, task));
        if (UserObjectiveTaskCompleted != null)
        {
            UserObjectiveTaskCompleted(objective, task);
        }
    }

    void objective_TaskAdded(GameplayObjective objective, ObjectiveTask newTask)
    {
        if (UserObjectiveTaskAdded != null)
        {
            UserObjectiveTaskAdded(objective, newTask);
        }
    }

    public GameplayObjective GetObjectiveByName(string name)
    {
        return _activeObjectives.Find((obj) => obj.Name == name);
    }

    public void RemoveObjective(GameplayObjective objective)
    {
        if (UserObjectiveRemoved != null)
        {
            UserObjectiveRemoved(objective);
        }
        _activeObjectives.Remove(objective);
    }
	
	public void Clear()
	{
		List<GameplayObjective> toRemove = new List<GameplayObjective>();
		toRemove.AddRange(_activeObjectives);
		
		foreach (var objective in toRemove)
		{
			objective.Complete();
		}
	}

    private void OnObjectiveCompleted(GameplayObjective objective)
    {
        AnalyticsLogger.Instance.AddLogEntry(new ObjectiveCompleteLogEntry(GameContext.Instance.Player.UserGuid, objective));
        if (UserObjectiveCompleted != null)
        {
            UserObjectiveCompleted(objective);
        }

        RemoveObjective(objective);
    }
}
