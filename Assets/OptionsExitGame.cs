using UnityEngine;

public class OptionsExitGame : MonoBehaviour
{
    public void ExitToMenuButtonPressed()
    {
	Application.LoadLevel("MainMenu");
    }

}
