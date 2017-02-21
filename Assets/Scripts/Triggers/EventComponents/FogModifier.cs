using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

public class FogModifier : MonoBehaviour
{
    private static FogModifier _instance;
    public static FogModifier Instance
    {
        get
        {
            if (!_instance)
            {
                GameObject go = new GameObject("FogModifier");
                _instance = go.AddComponent<FogModifier>();
            }
            return _instance;
        }
    }

    private Color _fogColor;
    private float _fogDensity;
    private float _fogStartDistance;
    private float _fogEndDistance;
    private Color _ambientLight;



    private bool _defaultEnabled;
    private Color _defaultFogColor;
    private float _defaultFogDensity;
    private float _defaultFogStartDistance;
    private float _defaultFogEndDistance;
    private UnityEngine.FogMode _defaultFogMode;
    private Color _defaultAmbientLight;

    private float _transitionRate = 0.15f;

    void Awake()
    {
        _defaultEnabled = RenderSettings.fog;
        _defaultFogColor = RenderSettings.fogColor;
        _defaultFogDensity = RenderSettings.fogDensity;
        _defaultFogStartDistance = RenderSettings.fogStartDistance;
        _defaultFogEndDistance = RenderSettings.fogEndDistance;
        _defaultFogMode = RenderSettings.fogMode;
        _defaultAmbientLight = RenderSettings.ambientLight;

        _fogColor = _defaultFogColor;
        _fogDensity = _defaultFogDensity;
        _fogStartDistance = _defaultFogStartDistance;
        _fogEndDistance = _defaultFogEndDistance;
        _ambientLight = _defaultAmbientLight;

        StartCoroutine(AdjustFogCoroutine());
    }

    public void SetTransitionRate(float transitionRate)
    {
        _transitionRate = transitionRate;
    }

    public void SetToDefault(bool smoothTransition)
    {
        RenderSettings.fog = _defaultEnabled;
        RenderSettings.fogMode = _defaultFogMode;

        if (!smoothTransition)
        {
            RenderSettings.fogColor = _defaultFogColor;
            RenderSettings.fogDensity = _defaultFogDensity;
            RenderSettings.fogEndDistance = _defaultFogEndDistance;
            RenderSettings.fogStartDistance = _defaultFogStartDistance;
            RenderSettings.ambientLight = _defaultAmbientLight;
        }

        _fogColor = _defaultFogColor;
        _fogDensity = _defaultFogDensity;
        _fogStartDistance = _defaultFogStartDistance;
        _fogEndDistance = _defaultFogEndDistance;
        _ambientLight = _defaultAmbientLight;
    }

    public void SetTargetFogSettings(bool enabled, UnityEngine.FogMode fogMode, Color fogColor, float fogDensity, float fogStartDistance, float fogEndDistance, Color ambientColor, bool smoothTransition)
    {
        RenderSettings.fog = enabled;
        RenderSettings.fogMode = fogMode;

        if (!smoothTransition)
        {
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.fogEndDistance = fogEndDistance;
            RenderSettings.fogStartDistance = fogStartDistance;
            RenderSettings.ambientLight = ambientColor;
        }

        _fogColor = fogColor;
        _fogDensity = fogDensity;
        _fogStartDistance = fogStartDistance;
        _fogEndDistance = fogEndDistance;
        _ambientLight = ambientColor;
    }

    private IEnumerator AdjustFogCoroutine()
    {
        while (true)
        {
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, _fogColor, Time.deltaTime * _transitionRate);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, _fogDensity, Time.deltaTime * _transitionRate);
            RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, _fogEndDistance, Time.deltaTime * _transitionRate);
            RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, _fogStartDistance, Time.deltaTime * _transitionRate);
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, _ambientLight, Time.deltaTime * _transitionRate);
            yield return null;
        }
    }
}
