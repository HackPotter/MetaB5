using UnityEngine;

public class CameraGraphics : MonoBehaviour
{
    private bool hasBloom;
    private bool hasVignetting;
    private bool hasNoise;

    private BloomAndLensFlares bloomScript;
    private Vignetting vignetteScript;
    private NoiseEffect noiseScript;

    void Start()
    {
        bloomScript = GetComponent<BloomAndLensFlares>();
        vignetteScript = GetComponent<Vignetting>();
        noiseScript = GetComponent<NoiseEffect>();

        if (bloomScript)
            hasBloom = true;
        else
            hasBloom = false;

        if (vignetteScript)
            hasVignetting = true;
        else
            hasVignetting = false;

        if (noiseScript)
            hasNoise = true;
        else
            hasNoise = false;
    }

    private void CheckGraphics()
    {
        // TODO upgrade this
#pragma warning disable 0618
        if (QualitySettings.currentLevel <= QualityLevel.Fast)
        {
            if (hasBloom)
                bloomScript.enabled = false;
            if (hasVignetting)
                vignetteScript.enabled = false;
            if (hasNoise)
                noiseScript.enabled = false;
        }
        else if (QualitySettings.currentLevel <= QualityLevel.Good)
        {
            if (hasBloom)
                bloomScript.enabled = true;
            if (hasVignetting)
                vignetteScript.enabled = true;
            if (hasNoise)
                noiseScript.enabled = true;
        }
        else if (QualitySettings.currentLevel <= QualityLevel.Fantastic)
        {
            if (hasBloom)
                bloomScript.enabled = true;
            if (hasVignetting)
                vignetteScript.enabled = true;
            if (hasNoise)
                noiseScript.enabled = true;
        }
#pragma warning restore 0618
    }
}