using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsExitGame : MonoBehaviour {
    public void ExitToMenuButtonPressed() {
        SceneManager.LoadScene("MainMenu");
    }
}
