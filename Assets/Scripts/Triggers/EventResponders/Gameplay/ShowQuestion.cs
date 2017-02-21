using UnityEngine;

public class ShowQuestion : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private QuestionData _questionData;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        MetablastUI.Instance.QuestionView.Show(_questionData);
    }
}

