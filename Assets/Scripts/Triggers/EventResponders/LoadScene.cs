using UnityEngine;

[Trigger(DisplayPath = "Scene")]
public class LoadScene : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _sceneToLoad;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
		// TODO hack
        if (MetablastUI.Instance)
        {
            MetablastUI.Instance.HudView.Hide(() => { }, 0.1f);
        }
        Application.LoadLevel(_sceneToLoad);
    }
}

