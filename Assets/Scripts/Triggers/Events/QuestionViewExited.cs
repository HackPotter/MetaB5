using System;

public class QuestionViewExited : EventSender
{
    protected override void OnStart()
    {
        base.OnStart();
        MetablastUI.Instance.QuestionView.QuestionViewExited += QuestionView_QuestionViewExited;
    }

    void OnDestroy()
    {
        MetablastUI.Instance.QuestionView.QuestionViewExited -= QuestionView_QuestionViewExited;
    }

    void QuestionView_QuestionViewExited()
    {
        TriggerEvent();
    }
}
