using UnityEngine;

public class DeleteSaveFileCheat : MonoBehaviour
{
    public void DeletePlayerSave()
    {
        GameContext.Instance.Player.PersistentStorage.ClearData();
        GameContext.Instance.Player.CurrentObjectives.Clear();
        Loader();
    }
    public void Loader()
    {
        Application.LoadLevel("SceneLoader");
    }
}
