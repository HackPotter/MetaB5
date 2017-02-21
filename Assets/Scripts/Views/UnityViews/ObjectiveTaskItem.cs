using UnityEngine;
using UnityEngine.UI;

namespace Metablast.UI
{
    public class ObjectiveTaskItem : MonoBehaviour
    {
        private ObjectiveTask _objectiveTask;
        private Animation _animation;

        public Text ObjectiveTaskText;
        public AnimationClip TaskCompleteAnimation;


        public ObjectiveTask ObjectiveTask
        {
            get { return _objectiveTask; }
            set
            {
                if (_objectiveTask != null)
                {
                    DebugFormatter.LogError(this, "Attemting to set ObjectiveTask on ObjectiveTaskItem, but ObjectiveTaskItem already has an ObjectiveTask.");
                    return;
                }
                _objectiveTask = value;
                ObjectiveTaskText.text = _objectiveTask.Name;
                _objectiveTask.TaskCompleted += _objectiveTask_TaskCompleted;
            }
        }

        void Awake()
        {
            _animation = GetComponent<Animation>();
        }

        void OnDestroy()
        {
            _objectiveTask.TaskCompleted -= _objectiveTask_TaskCompleted;
        }

        // TODO play an animation or something.
        void _objectiveTask_TaskCompleted(ObjectiveTask task)
        {
            if (_animation && TaskCompleteAnimation)
            {
                if (!_animation.Play(TaskCompleteAnimation.name))
                {
                    ObjectiveTaskText.fontStyle = FontStyle.Italic;
                }
                return;
            }
            ObjectiveTaskText.fontStyle = FontStyle.Italic;
        }

        public void DestroyTaskItem()
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
