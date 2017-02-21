using System;
using UnityEngine;
using UnityEngine.Events;

namespace Metablast.UI
{
    public interface IScreen
    {
        void Show(Action onComplete, float animationTime = 0.4f);
        void Hide(Action onComplete, float animationTime = 0.4f);
    }

    public class UIScreenDisplayEvent : UnityEvent<UIScreen> { }
	public abstract class UIScreen : MonoBehaviour, IScreen
	{

        public abstract void Show(Action onComplete, float animationTime = 0.4f);
        public abstract void Hide(Action onComplete, float animationTime = 0.4f);
	}
}
