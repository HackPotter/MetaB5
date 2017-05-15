using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchCredits : MonoBehaviour {

    public void Launch()
    {
        SceneManager.LoadScene("Credits Redux");
    }
}
