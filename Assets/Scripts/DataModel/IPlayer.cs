using System;

[Serializable]
public enum ActiveTool
{
    None,
    Scanner,
    ImpulseBeam
}

public delegate void FuelChanged(float newLevel);
public delegate void ToolStateChanged(ActiveTool activeTool);
public delegate void LightStateChanged(bool state);
public delegate void PointsChangedHandler(int points);


public interface IPlayer
{
    event FuelChanged OnATPChanged;
    event FuelChanged OnO2Changed;
    event FuelChanged OnNADPHChanged;
    event ToolStateChanged OnToolStateChanged;
    event LightStateChanged OnLightStateChanged;
	event PointsChangedHandler OnPointsChanged;
    event Action<bool> ImpulseBeamStateChanged;
    event Action<bool> ScannerStateChanged;


    // Profile
    string PlayerName { get; set; }
    Guid UserGuid { get; }
    
    // Resources
    float ATP { get; set; }
    float NADPH { get; set; }
    float O2 { get; set; }

    // Tools
    ActiveTool ActiveTool { get; set; }
    bool LightEnabled { get; set; }
    ImpulseBeamGrabFunction ImpulseBeamTool { get; set; }


    IGameplayObjectManager GameplayObjectManager { get; }
    IPersistentDataStorage PersistentStorage { get; }
    IPersistentDataStorage SessionStorage { get; }
    IUserObjectives CurrentObjectives { get; }
    IUserObjectives CompletedObjectives { get; }
    IEquippedTools Tools { get; }
    IBiologProgress BiologProgress { get; }
    PlayerQuestionProgress QuestionProgress { get; }

    int Points { get; set; }
}