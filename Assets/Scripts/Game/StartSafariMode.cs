using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSafariMode : MonoBehaviour
{

    public void Launch()
    {
        SceneManager.LoadScene("Cell");
    }
}
