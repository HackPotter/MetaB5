using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class BooLUnityEvent : UnityEvent<bool> { }

[Serializable]
public class ToolChangedEvent : UnityEvent<ActiveTool> { }
[Serializable]
public class LightStatusChangedEvent : UnityEvent<bool> { }
[Serializable]
public class ResourceValueChangedEvent : UnityEvent<float> { }

[Serializable]
public class GrabbedObjectEvent : UnityEvent<GrabbableObject> { }
[Serializable]
public class PlayerPointsEvent : UnityEvent<int> { }

[Serializable]
public class UserObjectiveEvent : UnityEvent<GameplayObjective> { }
[Serializable]
public class ObjectiveTaskEvent : UnityEvent<GameplayObjective, ObjectiveTask> { }
[Serializable]
public class BiologEntryScannedEvent : UnityEvent<BiologEntry> { }

[Serializable]
public class PointsChangedEvent : UnityEvent<int> {}

[Serializable]
public class ResourceEvents
{
    public ResourceValueChangedEvent ATPChanged;
    public ResourceValueChangedEvent NADPHChanged;
    public ResourceValueChangedEvent O2Changed;

	public PointsChangedEvent PointsChanged;
}

[Serializable]
public class ToolEvents
{
    public ToolChangedEvent ToolChanged;
    public LightStatusChangedEvent LightStatusChanged;
    public GrabbedObjectEvent PlayerGrabbedObject;
    public GrabbedObjectEvent PlayerDroppedObject;

    public BooLUnityEvent ImpulseBeamStateChanged;
    public BooLUnityEvent ScannerStateChanged;
}

[Serializable]
public class ObjectiveEvents
{
    public UserObjectiveEvent ObjectiveAdded;
    public UserObjectiveEvent ObjectiveCompleted;
    public ObjectiveTaskEvent ObjectiveTaskAdded;
    public ObjectiveTaskEvent ObjectiveTaskCompleted;
}

[Serializable]
public class BiologEvents
{
    public BiologEntryScannedEvent BiologEntryScanned;
    public BiologEntryScannedEvent BiologEntryUnlocked;
    public BiologEntryScannedEvent BiologEntryUnlockedQuiet;
}

public class GameFunctions : MonoBehaviour
{
    public ResourceEvents ResourceEvents;
    public ToolEvents ToolEvents;
    public ObjectiveEvents ObjectiveEvents;
    public BiologEvents BiologEvents;

    void Start()
    {
        AddToolListeners();
        AddResourceListeners();
        AddObjectiveListeners();
        AddBiologListeners();
    }

    void OnDestroy()
    {
        RemoveToolListeners();
        RemoveResourceListeners();
        RemoveObjectiveListeners();
        RemoveBiologListeners();
    }

    private void AddToolListeners()
    {
        GameContext.Instance.Player.OnToolStateChanged += ToolEvents.ToolChanged.Invoke;
        GameContext.Instance.Player.OnLightStateChanged += ToolEvents.LightStatusChanged.Invoke;
        GameContext.Instance.Player.ImpulseBeamStateChanged += Player_ImpulseBeamStateChanged;
        GameContext.Instance.Player.ScannerStateChanged += Player_ScannerStateChanged;

        GameContext.Instance.Player.ImpulseBeamTool.PlayerGrabbedObject += ToolEvents.PlayerGrabbedObject.Invoke;
        GameContext.Instance.Player.ImpulseBeamTool.PlayerDroppedObject += ToolEvents.PlayerDroppedObject.Invoke;
    }

    private void Player_ScannerStateChanged(bool obj)
    {
        ToolEvents.ScannerStateChanged.Invoke(obj);
    }

    private void Player_ImpulseBeamStateChanged(bool obj)
    {
        ToolEvents.ImpulseBeamStateChanged.Invoke(obj);
    }
    

    private void AddResourceListeners()
    {
        GameContext.Instance.Player.OnATPChanged += ResourceEvents.ATPChanged.Invoke;
        GameContext.Instance.Player.OnNADPHChanged += ResourceEvents.NADPHChanged.Invoke;
        GameContext.Instance.Player.OnO2Changed += ResourceEvents.O2Changed.Invoke;

		GameContext.Instance.Player.OnPointsChanged += ResourceEvents.PointsChanged.Invoke;
    }

    private void AddObjectiveListeners()
    {
        // Objectives
        GameContext.Instance.Player.CurrentObjectives.UserObjectiveAdded += ObjectiveEvents.ObjectiveAdded.Invoke;
        GameContext.Instance.Player.CurrentObjectives.UserObjectiveCompleted += ObjectiveEvents.ObjectiveAdded.Invoke;
        GameContext.Instance.Player.CurrentObjectives.UserObjectiveTaskAdded += ObjectiveEvents.ObjectiveTaskAdded.Invoke;
        GameContext.Instance.Player.CurrentObjectives.UserObjectiveTaskCompleted += ObjectiveEvents.ObjectiveTaskCompleted.Invoke;
    }

    private void AddBiologListeners()
    {
        GameContext.Instance.Player.BiologProgress.BiologEntryScanned += BiologEntryScannedHandler;
        GameContext.Instance.Player.BiologProgress.BiologEntryUnlocked += BiologEntryUnlockedHandler;
    }

    private void RemoveToolListeners()
    {
        GameContext.Instance.Player.OnToolStateChanged -= ToolEvents.ToolChanged.Invoke;
        GameContext.Instance.Player.OnLightStateChanged -= ToolEvents.LightStatusChanged.Invoke;

        GameContext.Instance.Player.ImpulseBeamStateChanged -= Player_ImpulseBeamStateChanged;
        GameContext.Instance.Player.ScannerStateChanged -= Player_ScannerStateChanged;

        GameContext.Instance.Player.ImpulseBeamTool.PlayerGrabbedObject -= ToolEvents.PlayerGrabbedObject.Invoke;
        GameContext.Instance.Player.ImpulseBeamTool.PlayerDroppedObject -= ToolEvents.PlayerDroppedObject.Invoke;
    }

    private void RemoveResourceListeners()
    {
        GameContext.Instance.Player.OnATPChanged -= ResourceEvents.ATPChanged.Invoke;
        GameContext.Instance.Player.OnNADPHChanged -= ResourceEvents.NADPHChanged.Invoke;
        GameContext.Instance.Player.OnO2Changed -= ResourceEvents.O2Changed.Invoke;


		GameContext.Instance.Player.OnPointsChanged -= ResourceEvents.PointsChanged.Invoke;
    }

    private void RemoveObjectiveListeners()
    {
        // Objectives
        GameContext.Instance.Player.CurrentObjectives.UserObjectiveAdded -= ObjectiveEvents.ObjectiveAdded.Invoke;
        GameContext.Instance.Player.CurrentObjectives.UserObjectiveCompleted -= ObjectiveEvents.ObjectiveAdded.Invoke;
        GameContext.Instance.Player.CurrentObjectives.UserObjectiveTaskAdded -= ObjectiveEvents.ObjectiveTaskAdded.Invoke;
        GameContext.Instance.Player.CurrentObjectives.UserObjectiveTaskCompleted -= ObjectiveEvents.ObjectiveTaskCompleted.Invoke;
    }

    private void RemoveBiologListeners()
    {
        GameContext.Instance.Player.BiologProgress.BiologEntryScanned -= BiologEntryScannedHandler;
        GameContext.Instance.Player.BiologProgress.BiologEntryUnlocked -= BiologEntryUnlockedHandler;
    }


    private void BiologEntryScannedHandler(BiologEntry scannedEntry, bool notify)
    {
        if (notify)
        {
            BiologEvents.BiologEntryScanned.Invoke(scannedEntry);
        }
    }

    private void BiologEntryUnlockedHandler(BiologEntry unlockedEntry, bool notify)
    {
        if (notify)
        {
            BiologEvents.BiologEntryUnlocked.Invoke(unlockedEntry);
        }
        else
        {
            BiologEvents.BiologEntryUnlockedQuiet.Invoke(unlockedEntry);
        }
    }

    public void PauseGame()
    {
        GameState.Instance.PauseLevel = PauseLevel.Cutscene;
    }

    public void UnpauseGame()
    {
        GameState.Instance.PauseLevel = PauseLevel.Unpaused;
    }

    public void SetToolImpulseBeam(bool enabled)
    {
        if (enabled)
        {
            SetTool(ActiveTool.ImpulseBeam);
        }
        else
        {
            SetTool(ActiveTool.None);
        }
    }

    public void SetToolScanner(bool enabled)
    {
        if (enabled)
        {
            SetTool(ActiveTool.Scanner);
        }
        else
        {
            SetTool(ActiveTool.None);
        }
    }

    public void SetTool(ActiveTool tool)
    {
        GameContext.Instance.Player.ActiveTool = tool;
    }

    public void SetATP(float atp)
    {
        GameContext.Instance.Player.ATP = atp;
    }

    public void AddATP(float amount)
    {
        GameContext.Instance.Player.ATP += amount;
    }

    public void SetNADPH(float nadph)
    {
        GameContext.Instance.Player.NADPH = nadph;
    }

    public void AddNADPH(float amount)
    {
        GameContext.Instance.Player.NADPH += amount;
    }

    public void SetO2(float o2)
    {
        GameContext.Instance.Player.O2 = o2;
    }

    public void AddO2(float amount)
    {
        GameContext.Instance.Player.O2 += amount;
    }

    public void AddPoints(int points)
    {
        GameContext.Instance.Player.Points += points;
    }

    public void SetPlayerName(string name)
    {
        GameContext.Instance.Player.PlayerName = name;
    }

    public void SetLight(bool on)
    {
        //Tools

        // Biolog Data





        //GameContext.Instance.Player.Inventory.AddItem(null);
        //GameContext.Instance.Player.Inventory.GetItemByName("");
        //GameContext.Instance.Player.Inventory.OnInventoryItemAdded;
        //GameContext.Instance.Player.Inventory.OnInventoryItemRemoved;



        GameContext.Instance.Player.LightEnabled = on;
    }

    public void UnlockBiologEntry(string entry)
    {
        GameContext.Instance.Player.BiologProgress.UnlockEntry(entry);
    }

    public void UnlockBiologEntryQuiet(string entry)
    {
        GameContext.Instance.Player.BiologProgress.UnlockEntryQuiet(entry);
    }
}

