using System;

public enum BiologLoadingProcess {
    UseProgressData,
    UnlockAll,
    UnlockNone,
}

[Serializable]
public class MockSessionData {
    public string Key;
    public string Value;
}

