using UnityEngine;

[Trigger(Description = "Sets the objective with the given name to be completed.", DisplayPath = "Objectives")]
[AddComponentMenu("Metablast/Triggers/Actions/Objectives/Complete Objective")]
public class CompleteObjective : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The name of the objective to be completed.")]
    private string _objectiveToComplete;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        GameplayObjective objectiveToComplete = GameContext.Instance.Player.CurrentObjectives.ActiveObjectives.Find((obj) => obj.Name == _objectiveToComplete);
        if (objectiveToComplete == null)
        {
            DebugFormatter.LogError(this, "Could not find objective {0}", _objectiveToComplete);
            return;
        }
        objectiveToComplete.Complete();
    }
}

