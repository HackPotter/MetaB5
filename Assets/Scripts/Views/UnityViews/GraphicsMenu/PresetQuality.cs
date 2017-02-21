using UnityEngine;
using System.Collections;

public class PresetQuality : MonoBehaviour {
    
    static string[] settingNames = UnityEngine.QualitySettings.names;

    int Low = 0;
    int Medium = settingNames.Length / 2;
    int High = settingNames.Length - 1;
    

    // This should take a QualityBehavior component and set the quality on it.
    public void SetLow()
    {
        // In Quality Settings UI, expose:
        //UnityEngine.QualitySettings.anisotropicFiltering;
        //UnityEngine.QualitySettings.antiAliasing;
        //UnityEngine.QualitySettings.pixelLightCount;
        //UnityEngine.QualitySettings.masterTextureLimit;
        //UnityEngine.QualitySettings.shadowDistance;
        //UnityEngine.QualitySettings.shadowCascades;
        //UnityEngine.QualitySettings.vSyncCount;

        
        UnityEngine.QualitySettings.SetQualityLevel(Low);
        Debug.Log("Set Quality to Low");
    }

    public void SetMedium()
    {
        UnityEngine.QualitySettings.SetQualityLevel(Medium);
        Debug.Log("Set Quality to Medium");
    }

    public void SetHigh()
    {
        UnityEngine.QualitySettings.SetQualityLevel(High);
        Debug.Log("Set Quality to High");

        
    }


    public void SetPreset()
    {
        if (UnityEngine.SystemInfo.processorCount <= 2 || UnityEngine.SystemInfo.systemMemorySize < 4100)
        {
            UnityEngine.QualitySettings.SetQualityLevel(0);
        }
        else if (UnityEngine.SystemInfo.processorCount >= 4 && UnityEngine.SystemInfo.systemMemorySize > 12000)
        {
            UnityEngine.QualitySettings.SetQualityLevel(settingNames.Length);
        }
        else
        {
            UnityEngine.QualitySettings.SetQualityLevel(settingNames.Length/2);
        }
        Debug.Log("Set Quality to Preset");
    }
}
