using Newtonsoft.Json;
using System;

public class SceneLoadedLogEntry : AnalyticsLogEntry
{
    private string _data;

    public override LogEntryType LogEntryType
    {
        get { return LogEntryType.SceneLoaded; }
    }

    public override string Data
    {
        get { return _data; }
    }

    public SceneLoadedLogEntry(Guid userGuid, string sceneName, int sceneIndex)
        : base(userGuid)
    {
        _data = JsonConvert.SerializeObject(new { SceneName = sceneName, SceneIndex = sceneIndex});
    }

    public SceneLoadedLogEntry(Guid userGuid, string sceneName, int sceneIndex, float overrideTime)
        : base(userGuid, overrideTime)
    {
        _data = JsonConvert.SerializeObject(new { SceneName = sceneName, SceneIndex = sceneIndex });
    }
}

