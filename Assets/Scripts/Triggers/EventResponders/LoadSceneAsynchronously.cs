using UnityEngine;
using UnityEngine.SceneManagement;

[Trigger(DisplayPath = "Unfinished")]
public class LoadSceneAsynchronously : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _sceneToLoad;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneToLoad);
        operation.allowSceneActivation = false;
    }
}

