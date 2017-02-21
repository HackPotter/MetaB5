using System;
using UnityEngine;

public enum TimerMode
{
    CountDown,
    Stopwatch
}

public class GameContext
{
    private static GameContext _instance;

    public static GameContext Instance
    {
        get
        {
            Initialize();
            return _instance;
        }
    }

    private static bool _initialized = false;
    private static void Initialize()
    {
        if (_initialized)
            return;

        _initialized = true;
        _instance = new GameContext(new MockDataProvider(new GameData(), BiologLoadingProcess.UseProgressData));
    }
    
    private GameContext(IPlayerDataProvider dataProvider)
    {
        GameData = dataProvider.GameData;
        Player = dataProvider.PlayerData;
    }


    private TimerMode _timerMode;
    private bool _running = false;
    private float _timerStartTime;

    private int _timerMinutes;
    private int _timerSeconds;

    public event Action TimerExpired;

    public void SetTimer(int minutes, int seconds)
    {
        _running = true;
        _timerMode = TimerMode.CountDown;
        _timerMinutes = minutes;
        _timerSeconds = seconds;
        _timerStartTime = Time.time;
    }

    public void SetStopwatch()
    {
        _timerMode = TimerMode.Stopwatch;
        _running = true;
        _timerStartTime = Time.time;
    }

    public void GetTimerState(out int minutes, out int seconds)
    {
        float timeSinceStarted = Time.time - _timerStartTime;


        switch (_timerMode)
        {
            case TimerMode.CountDown:
                float countdownSeconds = _timerMinutes * 60 + _timerSeconds;
                float timeRemaining = Mathf.Max(countdownSeconds - timeSinceStarted, 0);
                minutes = (int)(timeRemaining / 60);
                seconds = (int)timeRemaining % 60;
                break;
            case TimerMode.Stopwatch:
                minutes = (int)(timeSinceStarted / 60);
                seconds = (int)timeSinceStarted % 60;
                break;
        }

        minutes = 0;
        seconds = 0;
    }

    public IPlayer Player
    {
        get;
        private set;
    }

    public IGameData GameData
    {
        get;
        private set;
    }
}

