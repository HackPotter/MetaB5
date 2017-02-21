using Newtonsoft.Json;
using System;
using UnityEngine;

public class ObjectScannedLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.ObjectScanned; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public ObjectScannedLogEntry(Guid userGuid, ScannableObject scannedObject)
        : base(userGuid)
    {
        Vector3 position = scannedObject.transform.position;
        _data = JsonConvert.SerializeObject(
            new
            {
                BiologEntry = scannedObject.BiologEntry,
                GameObject = new
                {
                    Name = scannedObject.gameObject.name,
                    Location = new { X = position.x, Y = position.y, Z = position.z }
                }
            });
    }
}

