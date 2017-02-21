using UnityEngine;

[Trigger(Description = "Sets the global fog parameters.")]
public class SetFog : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("Whether or not the fog is enabled.")]
    private bool _enabled;
    [SerializeField]
    [Infobox("The color of the fog.")]
    private Color _fogColor;
    [SerializeField]
    [Infobox("The density of the fog.")]
    private float _fogDensity;
    [SerializeField]
    [Infobox("The minimum distance at which fog will be visible.")]
    private float _fogStartDistance;
    [SerializeField]
    [Infobox("The distance at which the fog will be at its maximum density.")]
    private float _fogEndDistance;
    [SerializeField]
    [Infobox("The method used to calculate fog.")]
    private UnityEngine.FogMode _fogMode;
    [SerializeField]
    [Infobox("The ambient color of the scene.")]
    private Color _ambientLight;
    [SerializeField]
    [Infobox("Whether or not the transition should be smooth.")]
    private bool _smoothTransition = true;
    [SerializeField]
    [Infobox("If the transition is set to smooth, this is the rate at which the parameters will transition.")]
    private float _transitionRate = 0.15f;
    [SerializeField]
    [Infobox("If true, the fog settings will be reset to default.")]
    private bool _setToDefault = false;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        FogModifier.Instance.SetTransitionRate(_transitionRate);
        if (_setToDefault)
        {
            FogModifier.Instance.SetToDefault(_smoothTransition);
        }
        else
        {
            FogModifier.Instance.SetTargetFogSettings(_enabled, _fogMode, _fogColor, _fogDensity, _fogStartDistance, _fogEndDistance, _ambientLight, _smoothTransition);
        }
    }


}

