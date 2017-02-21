
[Trigger(Description="Hides the dialogue box.", DisplayPath="Dialogue")]
public class ClearDialogue : EventResponder
{
    public override void OnEvent(ExecutionContext context)
    {
        if (MetablastUI.Instance != null)
        {
            MetablastUI.Instance.DialogueFrameView.Hide();
        }
    }
}