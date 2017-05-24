using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WindowsInput;

public class StartSafariMode : MonoBehaviour
{

    public void Launch()
    {
        InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_0);
        InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_Q);


    }
}
