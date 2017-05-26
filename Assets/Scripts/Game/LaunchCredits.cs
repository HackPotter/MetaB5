using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchCredits : MonoBehaviour {

    public string sceneToLoad;

    public void Launch()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
