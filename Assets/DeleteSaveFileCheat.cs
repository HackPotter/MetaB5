using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteSaveFileCheat : MonoBehaviour {
    public void DeletePlayerSave() {
        GameContext.Instance.Player.PersistentStorage.ClearData();
        GameContext.Instance.Player.CurrentObjectives.Clear();
        Loader();
    }
    public void Loader() {
        SceneManager.LoadScene("SceneLoader");
    }
}
