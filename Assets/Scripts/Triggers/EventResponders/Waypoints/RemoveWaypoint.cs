using UnityEngine;

[Trigger(DisplayPath = "Waypoint")]
public class RemoveWaypoint : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _identifier;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        base.TriggerRoot.WaypointView.RemoveWaypoint(_identifier);
    }
}