using UnityEngine;

[Trigger(Description = "Adds an objective to the player's active objectives.", DisplayPath="Objectives")]
[AddComponentMenu("Metablast/Triggers/Actions/Objectives/Add Objective")]
public class AddObjective : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private GameplayObjective _objective;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        GameplayObjective clone = new GameplayObjective();
        clone.Name = _objective.Name;
        clone.Description = _objective.Description;

        foreach (var task in _objective.Tasks)
        {
            var cloneTask = new ObjectiveTask();
            cloneTask.Name = task.Name;
            cloneTask.Description = task.Description;

            clone.AddTask(cloneTask);
        }
        GameContext.Instance.Player.CurrentObjectives.AddObjective(clone);
    }
}

