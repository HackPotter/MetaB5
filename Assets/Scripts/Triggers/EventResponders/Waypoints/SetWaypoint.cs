using UnityEngine;

[Trigger(DisplayPath = "Waypoint")]
public class SetWaypoint : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _identifier;
    [SerializeField]
    private Waypoint _waypoint;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        TriggerRoot.WaypointView.ClearWaypoints();
        TriggerRoot.WaypointView.AddWaypoint(_identifier, _waypoint);
    }
}

