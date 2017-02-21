using UnityEngine;

[Trigger(DisplayPath = "Unfinished")]
public class LoadSceneAsynchronously : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _sceneToLoad;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        AsyncOperation operation = Application.LoadLevelAsync(_sceneToLoad);
        operation.allowSceneActivation = false;
    }
}

