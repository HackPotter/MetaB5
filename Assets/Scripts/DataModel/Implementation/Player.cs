using UnityEngine;
using System;

public class Player : IPlayer
{
    private float _atp = 1;
    private float _nadph = 1;
    private float _o2 = 1;

	private int _points;

    private ActiveTool _activeTool = ActiveTool.None;
    private bool _lightEnabled = false;

    public Player()
    {
        UserGuid = Guid.NewGuid();
    }

    public string PlayerName
    {
        get;
        set;
    }

    public Guid UserGuid
    {
        get;
        private set;
    }

    public float ATP
    {
        get
        {
            return _atp;
        }
        set
        {
            if (value <= 0 && _atp > 0)
            {
                AnalyticsLogger.Instance.AddLogEntry(new ResourceEventLogEntry(UserGuid, ResourceEventLogEntry.ResourceType.ATP, ResourceEventLogEntry.EventType.Empty));
            }
            if (value >= 1 && _atp < 1)
            {
                AnalyticsLogger.Instance.AddLogEntry(new ResourceEventLogEntry(UserGuid, ResourceEventLogEntry.ResourceType.ATP, ResourceEventLogEntry.EventType.Full));
            }
            _atp = value;

            _atp = Mathf.Clamp01(_atp);
            if (OnATPChanged != null)
            {
                OnATPChanged(_atp);
            }
        }
    }

    public float NADPH
    {
        get
        {
            return _nadph;
        }
        set
        {
            if (value <= 0 && _nadph > 0)
            {
                AnalyticsLogger.Instance.AddLogEntry(new ResourceEventLogEntry(UserGuid, ResourceEventLogEntry.ResourceType.NADPH, ResourceEventLogEntry.EventType.Empty));
            }
            if (value >= 1 && _nadph < 1)
            {
                AnalyticsLogger.Instance.AddLogEntry(new ResourceEventLogEntry(UserGuid, ResourceEventLogEntry.ResourceType.NADPH, ResourceEventLogEntry.EventType.Full));
            }
            _nadph = value;
            _nadph = Mathf.Clamp01(_nadph);
            if (OnNADPHChanged != null)
            {
                OnNADPHChanged(_nadph);
            }
        }
    }

    public float O2
    {
        get
        {
            return _o2;
        }
        set
        {
            if (value <= 0 && _o2 > 0)
            {
                AnalyticsLogger.Instance.AddLogEntry(new ResourceEventLogEntry(UserGuid, ResourceEventLogEntry.ResourceType.O2, ResourceEventLogEntry.EventType.Empty));
            }
            if (value >= 1 && _o2 < 1)
            {
                AnalyticsLogger.Instance.AddLogEntry(new ResourceEventLogEntry(UserGuid, ResourceEventLogEntry.ResourceType.O2, ResourceEventLogEntry.EventType.Full));
            }
            _o2 = value;
            _o2 = Mathf.Clamp01(_o2);
            if (OnO2Changed != null)
            {
                OnO2Changed(_o2);
            }
        }
    }

    public ActiveTool ActiveTool
    {
        get
        {
            return _activeTool;
        }
        set
        {
            if (_activeTool != value)
            {
                AnalyticsLogger.Instance.AddLogEntry(new ToolEnabledLogEntry(GameContext.Instance.Player.UserGuid, _activeTool));
            }
            else
            {
                return;
            }
            
            switch (_activeTool)
            {
                case ActiveTool.Scanner:
                    if (ScannerStateChanged != null)
                        ScannerStateChanged(false);
                    break;
                case ActiveTool.ImpulseBeam:
                    if (ImpulseBeamStateChanged != null)
                        ImpulseBeamStateChanged(false);
                    break;
            }

            _activeTool = value;

            switch (_activeTool)
            {
                case ActiveTool.Scanner:
                    if (ScannerStateChanged != null)
                        ScannerStateChanged(true);
                    break;
                case ActiveTool.ImpulseBeam:
                    if (ImpulseBeamStateChanged != null)
                        ImpulseBeamStateChanged(true);
                    break;
            }
            if (OnToolStateChanged != null)
            {
                OnToolStateChanged(_activeTool);
            }
        }
    }

    public bool LightEnabled
    {
        get
        {
            return _lightEnabled;
        }
        set
        {
            if (_lightEnabled != value)
            {
                AnalyticsLogger.Instance.AddLogEntry(new LightToolEnabledLogEntry(GameContext.Instance.Player.UserGuid, value));
            }
            _lightEnabled = value;
            if (OnLightStateChanged != null)
            {
                OnLightStateChanged(_lightEnabled);
            }
        }
    }

    public ImpulseBeamGrabFunction ImpulseBeamTool
    {
        get;
        set;
    }

    public event FuelChanged OnATPChanged;

    public event FuelChanged OnO2Changed;

    public event FuelChanged OnNADPHChanged;

    public event ToolStateChanged OnToolStateChanged;

    public event Action<bool> ImpulseBeamStateChanged;
    
    public event Action<bool> ScannerStateChanged;
    

    public event LightStateChanged OnLightStateChanged;
    
	public event PointsChangedHandler OnPointsChanged;
    

    public IUserObjectives CurrentObjectives
    {
        get;
        set;
    }

    public IUserObjectives CompletedObjectives
    {
        get;
        set;
    }

    public IEquippedTools Tools
    {
        get;
        set;
    }

    public IGameplayObjectManager GameplayObjectManager
    {
        get;
        set;
    }
    public IPersistentDataStorage PersistentStorage
    {
        get;
        set;
    }

    public IPersistentDataStorage SessionStorage
    {
        get;
        set;
    }

    public IBiologProgress BiologProgress
    {
        get;
        set;
    }

    public PlayerQuestionProgress QuestionProgress
    {
        get;
        set;
    }

    public int Points
    {
		get { return _points;}
		set
		{
			_points = value;
			if (OnPointsChanged != null)
				OnPointsChanged(_points);

		}
    }
}

