using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Trigger(Description = "Completes a task with the given name on an objective with the given name.", DisplayPath = "Objectives")]
[AddComponentMenu("Metablast/Triggers/Actions/Objectives/Complete Objective Task")]
public class CompleteObjectiveTask : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The name of the objective.")]
    private string _objectiveName;

    [SerializeField]
    [Infobox("The name of the task.")]
    private string _taskName;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        var objective = GameContext.Instance.Player.CurrentObjectives.GetObjectiveByName(_objectiveName);
        if (objective != null)
        {
            IEnumerable<ObjectiveTask> tasks = objective.Tasks.Where((t) => t.Name == _taskName);
            foreach (var task in tasks)
            {
                task.Complete();
            }
        }
    }
}

