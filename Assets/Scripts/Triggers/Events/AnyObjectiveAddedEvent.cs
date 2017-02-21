using UnityEngine;

[Trigger(Description="Invoked when the player has received any new objective.", DisplayPath="Objectives")]
[AddComponentMenu("Metablast/Triggers/Events/Objectives/Any Objective Added Event")]
public class AnyObjectiveAddedEvent : EventSender
{
    protected override void OnStart()
    {
        GameContext.Instance.Player.CurrentObjectives.UserObjectiveAdded += UserObjectives_UserObjectiveAdded;
    }

    void OnDestroy()
    {
        GameContext.Instance.Player.CurrentObjectives.UserObjectiveAdded -= UserObjectives_UserObjectiveAdded;
    }

    void UserObjectives_UserObjectiveAdded(GameplayObjective objective)
    {
        TriggerEvent();
    }
}
