using UnityEngine;

public class Pause : MonoBehaviour {

    public void PauseGame()
    {
        GameState.Instance.PauseLevel = PauseLevel.Menu;
    }
    public void UnPauseGame()
    {
        GameState.Instance.PauseLevel = PauseLevel.Unpaused;
    }
}
