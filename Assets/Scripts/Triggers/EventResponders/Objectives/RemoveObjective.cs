using UnityEngine;

[Trigger(Description = "Removes an objective with the given name. This will not count as completing the objective for the player.", DisplayPath = "Objectives")]
[AddComponentMenu("Metablast/Triggers/Actions/Objectives/Remove Objective")]
public class RemoveObjective : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The name of the objective to remove.")]
    private string _objectiveToRemove;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        GameplayObjective objectiveToComplete = GameContext.Instance.Player.CurrentObjectives.ActiveObjectives.Find((obj) => obj.Name == _objectiveToRemove);
        if (objectiveToComplete == null)
        {
            DebugFormatter.LogError(this, "Could not find objective {0}", _objectiveToRemove);
            return;
        }
        GameContext.Instance.Player.CurrentObjectives.RemoveObjective(objectiveToComplete);
    }
}

