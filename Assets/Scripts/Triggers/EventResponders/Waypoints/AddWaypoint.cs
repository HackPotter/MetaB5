using UnityEngine;

[Trigger(Description = "Adds a waypoint with the given identifier.", DisplayPath = "Waypoint")]
public class AddWaypoint : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The identifier of the waypoint to be added. This can be used to remove or change the parameters of the waypoint later.")]
    private string _identifier;
    [SerializeField]
    private Waypoint _waypoint;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        base.TriggerRoot.WaypointView.AddWaypoint(_identifier, _waypoint);
    }
}

