using System.Collections.Generic;

public delegate void BiologEntryScannedHandler(BiologEntry unlockedEntry, bool notify);

public interface IBiologProgress
{
    event BiologEntryScannedHandler BiologEntryUnlocked;
    event BiologEntryScannedHandler BiologEntryScanned;
    bool IsEntryUnlocked(string entry);
    void UnlockEntry(string entryToUnlock);
    void UnlockEntryQuiet(string entryToUnlock);
    List<BiologEntry> UnlockedEntries { get; }
}