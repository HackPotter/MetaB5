using UnityEngine;

[Trigger(DisplayPath = "UI")]
[AddComponentMenu("Metablast/Triggers/Actions/UI/Show Context Message")]
public class ShowContextMessage : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
	private string _messageToShow;

    [SerializeField]
    private bool _setBackgroundColor;
    [SerializeField]
    private Color _backgroundColor;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        if (_setBackgroundColor)
        {
            MetablastUI.Instance.HudView.ContextMessageView.SetText(_messageToShow, _backgroundColor);
        }
        else
        {
            MetablastUI.Instance.HudView.ContextMessageView.SetText(_messageToShow);
        }
    }
}

