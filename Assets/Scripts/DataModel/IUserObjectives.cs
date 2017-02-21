using System.Collections.Generic;

public delegate void UserObjectiveAdded(GameplayObjective objective);
public delegate void UserObjectiveTaskAdded(GameplayObjective objective, ObjectiveTask task);
public delegate void UserObjectiveTaskCompleted(GameplayObjective objective, ObjectiveTask task);
public delegate void UserObjectiveRemoved(GameplayObjective objective);
public delegate void UserObjectiveCompleted(GameplayObjective objective);

public interface IUserObjectives
{
    event UserObjectiveAdded UserObjectiveAdded;
    event UserObjectiveRemoved UserObjectiveRemoved;
    event UserObjectiveCompleted UserObjectiveCompleted;
    event UserObjectiveTaskAdded UserObjectiveTaskAdded;
    event UserObjectiveTaskCompleted UserObjectiveTaskCompleted;

    List<GameplayObjective> ActiveObjectives { get; }
    void AddObjective(GameplayObjective objective);
    GameplayObjective GetObjectiveByName(string name);
    void RemoveObjective(GameplayObjective objective);
	void Clear();
}

