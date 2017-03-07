using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseMiniGames : MonoBehaviour {

    // Use this for initialization
    public void onClickExitButton()
    {
        SceneManager.LoadScene("Laboratory");
        Debug.Log("Close Button Clicked");
    }
}
