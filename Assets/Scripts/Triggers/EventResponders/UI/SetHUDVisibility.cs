using UnityEngine;

[Trigger(DisplayPath = "UI")]
public class SetHUDVisibility : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private bool _visible;
    [SerializeField]
    private bool _overrideDefaultAnimationTime;
    [SerializeField]
    private float _animationTime;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        if (_visible)
        {
			if  (MetablastUI.Instance.BiologView.IsShowing)
			{
				// do nothing because we don't want to cover up the biolog.
			}
            else if (_overrideDefaultAnimationTime)
            {
                MetablastUI.Instance.HudView.Show(() => { }, _animationTime);
            }
            else
            {
                MetablastUI.Instance.HudView.Show(() => { });
            }
        }
        else
        {
            if (_overrideDefaultAnimationTime)
            {
                MetablastUI.Instance.HudView.Hide(() => { }, _animationTime);
            }
            else
            {
                MetablastUI.Instance.HudView.Hide(() => { });
            }
        }
    }
}

