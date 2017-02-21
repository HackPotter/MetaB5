
[Trigger(DisplayPath = "Waypoint")]
public class ClearWaypoints : EventResponder
{
    public override void OnEvent(ExecutionContext context)
    {
        base.TriggerRoot.WaypointView.ClearWaypoints();
    }
}