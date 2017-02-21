using UnityEngine;
using System.Collections;

public class Toggles : MonoBehaviour {

   
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("Fullscreen Toggled");
    }

    public void ToggleVSync()
    {
        if (QualitySettings.vSyncCount != 0)
            QualitySettings.vSyncCount = 0;
        else
            QualitySettings.vSyncCount = 1;
        Debug.Log("VSync Toggled");
    }
}
