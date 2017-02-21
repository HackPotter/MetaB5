using UnityEngine;

[Trigger(Description = "Invoked after the player has completed an objective with the given objective name.", DisplayPath = "Objectives")]
[AddComponentMenu("Metablast/Triggers/Events/Objectives/Objective Completed")]
public class ObjectiveCompletedEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("When an objective is completed, if the name is this, then this event will be invoked.")]
    private string _objectiveName;
#pragma warning restore 0067, 0649

    protected override void OnStart()
    {
        GameContext.Instance.Player.CurrentObjectives.UserObjectiveCompleted += UserObjectives_UserObjectiveCompleted;
    }

    void OnDestroy()
    {
        GameContext.Instance.Player.CurrentObjectives.UserObjectiveCompleted -= UserObjectives_UserObjectiveCompleted;
    }

    void UserObjectives_UserObjectiveCompleted(GameplayObjective objective)
    {
        if (objective.Name == _objectiveName)
        {
            TriggerEvent();
        }
    }
}

