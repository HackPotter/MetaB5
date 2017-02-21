using Newtonsoft.Json;
using System;

public class ObjectDroppedLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.ObjectDropped; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public ObjectDroppedLogEntry(Guid userGuid, GrabbableObject grabbedObject)
        : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(
            new
            {
                GameObject = new
                {
                    Name = grabbedObject.gameObject.name,
                    Position = new
                    {
                        X = grabbedObject.transform.position.x,
                        Y = grabbedObject.transform.position.y,
                        Z = grabbedObject.transform.position.z
                    }
                }

            });
    }
}

