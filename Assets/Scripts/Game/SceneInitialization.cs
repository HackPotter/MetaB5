using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

//Keep GameContext, but use only for scene-independent stuff.
//Like, session data, biolog entries unlocked, all that jazz.
//Change SceneInitialization to SceneContext.
//Move SceneContext onto a GameObject with the ship prefab.

public class SceneInitialization : MonoBehaviour {
#pragma warning disable 0067, 0649
    [SerializeField]
    private bool _useMockData;
    [SerializeField]
    private BiologLoadingProcess _biologLoadingProcess;
    [SerializeField]
    private MockSessionData[] _mockSessionData;
#pragma warning restore 0067, 0649

    private static bool _sceneInitialized;
    private bool _initialize;

    void Awake() {
        if (_sceneInitialized) {
            GameObject.Destroy(this.gameObject);
            return;
        }
        else {
            _initialize = true;
            AnalyticsLogger.Instance.Load();

            OnLevelWasLoaded(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void ResetInitializer() {
        ApplicationEvents.Instance.ApplicationFocused -= Instance_ApplicationFocused;
        ApplicationEvents.Instance.ApplicationPaused -= Instance_ApplicationPaused;
        ApplicationEvents.Instance.ApplicationQuit -= Instance_ApplicationQuit;
    }

    void OnLevelWasLoaded(int level) {
        string currentScene = SceneManager.GetActiveScene().name;
        // Prevent existing SceneInitializers from executing.
        if (!_initialize) {
            return;
        }

        // Skip this if we've already initialized and we're not at the main menu.
        if (level != 0 && _sceneInitialized) {
            AnalyticsLogger.Instance.AddLogEntry(new SceneLoadedLogEntry(GameContext.Instance.Player.UserGuid, currentScene, level));
            return;
        }
        RememberMe.Clear();
        Screen.SetResolution(1280, 720, false, 60);

        DontDestroyOnLoad(this.gameObject);
        IGameData gameData = new GameData();


        if (MetablastUI.Instance != null) {
            MetablastUI.Instance.Destroy();
        }

        if (Debug.isDebugBuild) {
            MetablastLogger.Instance.Start();
            MetablastLogger.Instance.LogMessage(this, "Initializing Scene");
            MetablastLogger.Instance.LogMessage(this, "Screen Resolution Set: {0}x{1}", Screen.width, Screen.height);
        }

        ApplicationEvents.Instance.ApplicationFocused += new System.Action<bool>(Instance_ApplicationFocused);
        ApplicationEvents.Instance.ApplicationPaused += new System.Action<bool>(Instance_ApplicationPaused);
        ApplicationEvents.Instance.ApplicationQuit += new System.Action(Instance_ApplicationQuit);

        if (Application.isEditor && !_sceneInitialized) {
            GameObject debugViewObject = new GameObject();
            debugViewObject.transform.parent = this.transform;
            debugViewObject.AddComponent<DebugView>();
            DontDestroyOnLoad(debugViewObject);
        }

        if (!_sceneInitialized) {
            // Odd bug. Unity doesn't give back the right time in the first frame?
            AnalyticsLogger.Instance.AddLogEntry(new SceneLoadedLogEntry(GameContext.Instance.Player.UserGuid, currentScene, level, 0f));
        }
        else {
            AnalyticsLogger.Instance.AddLogEntry(new SceneLoadedLogEntry(GameContext.Instance.Player.UserGuid, currentScene, level));
        }
        _sceneInitialized = true;
    }

    void OnApplicationQuit() {
        AnalyticsLogger.Instance.ApplicationQuit();
    }

    void Update() {
        for (int i = 0; i < 3; i++) {
            if (Input.GetMouseButtonDown(i)) {
                AnalyticsLogger.Instance.AddLogEntry(new MouseClickLogEntry(GameContext.Instance.Player.UserGuid, (MouseButton)i, Input.mousePosition));
            }
        }
    }

    void Instance_ApplicationQuit() {
        // Temporary fix.
        //File.Delete("tempFile.txt");
        //File.Delete("biologProgress.txt");
        MetablastLogger.Instance.LogMessage(this, "Application Quit");
        MetablastLogger.Instance.End();
    }

    void Instance_ApplicationPaused(bool obj) {
        MetablastLogger.Instance.LogMessage(this, "Application Paused - {0}", obj);
    }

    void Instance_ApplicationFocused(bool obj) {
        MetablastLogger.Instance.LogMessage(this, "Application Focused - {0}", obj);
    }
}
