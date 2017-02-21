using System;

public interface IQuestionView
{
    event Action QuestionViewExited;
    void Show(QuestionData question);
}

