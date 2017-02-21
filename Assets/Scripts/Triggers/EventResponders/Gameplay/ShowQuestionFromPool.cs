using UnityEngine;

public class ShowQuestionFromPool : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
	private SceneCapsuleQuestionProvider _questionProvider;
	
	[SerializeField]
	private SubroutineEvent _noMoreQuestionsLeft;
	
	[SerializeField]
    [ExpressionField(typeof(GameObject),"Capsule")]
    private Expression _capsuleObject;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
	{
		GameObject gameObject = context.Evaluate<GameObject>(_capsuleObject);
		
		Debug.Log("CapsuleObject evaluated to " + gameObject);
		QuestionData questionData = _questionProvider.GetQuestionData(gameObject);
		
		if (questionData == null)
		{
			if (_noMoreQuestionsLeft != null)
			{
				_noMoreQuestionsLeft.InvokeSubroutine();
			}
			return;
		}
		
		QuestionProgress progress = GameContext.Instance.Player.QuestionProgress.GetQuestionProgress(questionData);
		if (progress != null && progress.Completed)
		{
			questionData = _questionProvider.GetNewQuestionData(gameObject);
		}
		
		if (questionData == null)
		{
			if (_noMoreQuestionsLeft != null)
			{
				_noMoreQuestionsLeft.InvokeSubroutine();
			}
			return;
		}
		MetablastUI.Instance.QuestionView.Show(questionData);
		
	}
}

