using UnityEngine;
using UnityEngine.UI;

namespace Metablast.UI
{
    public class ObjectiveListItem : MonoBehaviour
	{
        public Text ObjectiveName;
        public VerticalLayoutGroup ObjectiveTaskGroup;
        public ObjectiveTaskItem ObjectiveTaskItemPrefab;

        public AnimationClip ObjectiveCompleteAnimation;

        private Animation _animation;
        private GameplayObjective _objective;

        public GameplayObjective Objective
        {
            get { return _objective; }
            set
            {
                if (_objective != null)
                {
                    DebugFormatter.LogError(this, "Attempting to set the objective of ObjectiveListItem, but a GameplayObjective has already been set.");
                    return;
                }
                _objective = value;
                _objective.Completed += _objective_Completed;
                _objective.TaskAdded += _objective_TaskAdded;
                ObjectiveName.text = _objective.Name;

                foreach (var task in _objective.Tasks)
                {
                    AddTask(task);
                }   
            }
        }

        void Awake()
        {
            _animation = GetComponent<Animation>();
        }

        private void AddTask(ObjectiveTask task)
        {
            ObjectiveTaskItem objectiveTaskItem = GameObject.Instantiate(ObjectiveTaskItemPrefab) as ObjectiveTaskItem;

            objectiveTaskItem.ObjectiveTask = task;
            objectiveTaskItem.transform.SetParent(ObjectiveTaskGroup.transform);
            objectiveTaskItem.transform.localScale = Vector3.one;
        }

        void _objective_TaskAdded(GameplayObjective objective, ObjectiveTask task)
        {
            AddTask(task);
        }
        
        void _objective_Completed(GameplayObjective objective)
        {
            _objective.TaskAdded -= _objective_TaskAdded;
            _objective.Completed -= _objective_Completed;
            
            if (_animation && ObjectiveCompleteAnimation)
            {
                if (!_animation.Play(ObjectiveCompleteAnimation.name))
                {
                    DestroyObjectiveListItem();
                }
                return;
            }
            DestroyObjectiveListItem();
        }

        public void DestroyObjectiveListItem()
        {
            GameObject.Destroy(this.gameObject);
        }
	}
}
