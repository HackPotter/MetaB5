using UnityEngine;

[Trigger(Description = "Invoked after the player has completed a task with the given task name as part of the objective with the given objective name.", DisplayPath = "Objectives")]
[AddComponentMenu("Metablast/Triggers/Events/Objectives/Objective Task Completed")]
public class ObjectiveTaskCompletedEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("When an objective task is completed, this event will only be invoked if the objective name of the completed task is equal to the name provided here.")]
    private string _objectiveName;

    [SerializeField]
    [Infobox("When an objective task is completed, this event will only be invoked if the completed task's name is equal to the name provided here.")]
    private string _taskName;
#pragma warning restore 0067, 0649

    protected override void OnStart()
    {
        GameContext.Instance.Player.CurrentObjectives.GetObjectiveByName(_objectiveName).TaskCompleted += ObjectiveTaskCompletedEvent_TaskCompleted;
    }

    void OnDestroy()
    {
        GameplayObjective objective = GameContext.Instance.Player.CurrentObjectives.GetObjectiveByName(_objectiveName);
        if (objective != null)
        {
            objective.TaskCompleted -= ObjectiveTaskCompletedEvent_TaskCompleted;
        }
    }

    void ObjectiveTaskCompletedEvent_TaskCompleted(GameplayObjective objective, ObjectiveTask task)
    {
        if (task.Name == _taskName)
        {
            TriggerEvent();
        }
    }
}

