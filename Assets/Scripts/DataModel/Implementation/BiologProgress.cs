using System.Collections.Generic;

public class BiologProgress : IBiologProgress
{
    private List<BiologEntry> _unlockedEntries;
    private IPersistentDataStorage _dataStorage;

    public List<BiologEntry> UnlockedEntries
    {
        get { return _unlockedEntries; }
    }

    public BiologProgress(IGameData gameData)
    {
        _unlockedEntries = new List<BiologEntry>();

        _dataStorage = new TransientDataStorage();//"biologProgress.txt");
        _dataStorage.ReadData();

        foreach (KeyValuePair<string, object> value in _dataStorage)
        {
            _unlockedEntries.Add(gameData.BiologData.Entries.Find((e) => e.EntryName.Equals(value.Key)));
        }
    }

    public BiologProgress(IGameData gameData, BiologLoadingProcess loadingProcess)
    {
        _unlockedEntries = new List<BiologEntry>();
        _dataStorage = new TransientDataStorage();// PersistentDataStorage("biologProgress.txt");
        _dataStorage.ReadData();

        switch (loadingProcess)
        {
            case BiologLoadingProcess.UnlockAll:
                foreach (BiologEntry entry in gameData.BiologData.Entries)
                {
                    _unlockedEntries.Add(entry);
                }
                break;
            case BiologLoadingProcess.UnlockNone:
                break;
            case BiologLoadingProcess.UseProgressData:
                _unlockedEntries = new List<BiologEntry>();

                _dataStorage = new TransientDataStorage();//new PersistentDataStorage("biologProgress.txt");
                _dataStorage.ReadData();

                foreach (KeyValuePair<string, object> value in _dataStorage)
                {
                    _unlockedEntries.Add(gameData.BiologData.Entries.Find((e) => e.EntryName.Equals(value.Key)));
                }
                break;
        }
    }

    public bool IsEntryUnlocked(string entry)
    {
        return _unlockedEntries.Find((e) => e.EntryName == entry) != null;
    }

    public void UnlockEntry(string entryName)
    {
        BiologEntry entry = GameContext.Instance.GameData.BiologData.Entries.Find((e) => e.EntryName.Equals(entryName));

        if (entry != null)
        {
            if (!_unlockedEntries.Contains(entry))
            {
                AnalyticsLogger.Instance.AddLogEntry(new BiologEntryUnlockedLogEntry(GameContext.Instance.Player.UserGuid, entry));
                _unlockedEntries.Add(entry);
                if (!_dataStorage.HasKeyForString(entryName))
                {
                    _dataStorage.Store(entryName, "x");
                    _dataStorage.WriteData();
                }

                if (BiologEntryUnlocked != null)
                {
                    BiologEntryUnlocked(entry, true);
                }
            }
            if (BiologEntryScanned != null)
            {
                BiologEntryScanned(entry, true);
            }
        }
        else
        {
            DebugFormatter.LogError(this, "Attempting to unlock Biolog entry {0}, but entry could not be found in BiologData.", entryName);
        }
    }

    public event BiologEntryScannedHandler BiologEntryUnlocked;

    public event BiologEntryScannedHandler BiologEntryScanned;

    public void UnlockEntryQuiet(string entryName)
    {
        BiologEntry entry = GameContext.Instance.GameData.BiologData.Entries.Find((e) => e.EntryName.Equals(entryName));

        if (entry != null)
        {
            if (!_unlockedEntries.Contains(entry))
            {
                _unlockedEntries.Add(entry);
                if (!_dataStorage.HasKeyForString(entryName))
                {
                    _dataStorage.Store(entryName, "x");
                    _dataStorage.WriteData();
                }

                if (BiologEntryUnlocked != null)
                {
                    BiologEntryUnlocked(entry, false);
                }
            }
            if (BiologEntryScanned != null)
            {
                BiologEntryScanned(entry, false);
            }
        }
        else
        {
            DebugFormatter.LogError(this, "Attempting to unlock Biolog entry {0}, but entry could not be found in BiologData.", entryName);
        }
    }
}

