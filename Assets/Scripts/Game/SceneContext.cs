using UnityEngine;


public class SceneContext : MonoBehaviour
{
    [SerializeField]
    private MockSessionData[] _mockSessionData;

    void Awake()
    {
        RememberMe.Clear();
        Screen.SetResolution(1280,720,false,60);
        
        foreach (var sessionData in _mockSessionData)
        {
            GameContext.Instance.Player.SessionStorage.Store(sessionData.Key, sessionData.Value);
        }
    }
}
