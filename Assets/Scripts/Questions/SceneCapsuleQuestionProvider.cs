using System.Collections.Generic;
using UnityEngine;

public class SceneCapsuleQuestionProvider : MonoBehaviour
{
	private bool _hasInitialized = false;
	
	[SerializeField]
	private List<QuestionData> _questions = new List<QuestionData>();
	
	private Dictionary<GameObject, QuestionData> _questionsByCapsule = new Dictionary<GameObject, QuestionData>();
	
	
	public QuestionData GetNewQuestionData(GameObject gameObject)
	{
		_questionsByCapsule.Remove(gameObject);
		return GetQuestionData(gameObject);
	}
	
	public QuestionData GetQuestionData(GameObject gameObject)
	{
		if (!_hasInitialized)
		{
			List<QuestionData> toRemove = new List<QuestionData>();
			foreach (QuestionData q in _questions)
			{
				var questionProgress = GameContext.Instance.Player.QuestionProgress.GetQuestionProgress(q);
				if (questionProgress != null && questionProgress.Completed)
				{
					toRemove.Add(q);
				}
			}
			
			foreach (QuestionData questionToRemove in toRemove)
			{
				_questions.Remove(questionToRemove);
			}
			_hasInitialized = true;
		}
		
		
		QuestionData questionData;
		if (!_questionsByCapsule.TryGetValue(gameObject, out questionData))
		{
			if (_questions.Count == 0)
			{
				return null;
			}
			
			int questionIndex = Random.Range(0, _questions.Count);
			questionData = _questions[questionIndex];
		
			_questions.Remove(questionData);
			_questionsByCapsule.Add(gameObject, questionData);
		}
		
		return questionData;
	}
}
