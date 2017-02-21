using UnityEngine;

[Trigger(Description = "Adds an objective task to the given objective.", DisplayPath = "Objectives")]
[AddComponentMenu("Metablast/Triggers/Actions/Objectives/Add Objective Task")]
public class AddObjectiveTask : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The objective to which the task will be added.")]
    private string _objectiveName;
    [SerializeField]
    private ObjectiveTask _newTask;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        GameplayObjective objective = GameContext.Instance.Player.CurrentObjectives.GetObjectiveByName(_objectiveName);
        if (objective == null)
        {
            DebugFormatter.LogError(this, "Could not find objective by name {0}", _objectiveName);
            return;
        }

        objective.AddTask(_newTask);
    }
}

