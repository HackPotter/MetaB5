using UnityEngine;

public enum PauseLevel
{
    Unpaused = 0,
    Cutscene = 1,
    Dialogue = 2,
    Menu = 3
}

public delegate void PauseLevelChanged(PauseLevel pauseLevel);

public class GameState
{
    public event PauseLevelChanged OnPauseLevelChanged;

    private PauseLevel _currentPauseLevel;

    public static readonly GameState Instance = new GameState();

    private GameState()
    {

    }

    public PauseLevel PauseLevel
    {
        get
        {
            return _currentPauseLevel;
        }
        set
        {
            if (Debug.isDebugBuild)
            {
                MetablastLogger.Instance.LogMessage(this, "Pause Level Changed: {0}", _currentPauseLevel.ToString());
            }
            _currentPauseLevel = value;
            if (OnPauseLevelChanged != null)
            {
                OnPauseLevelChanged(_currentPauseLevel);
            }
        }
    }
}

