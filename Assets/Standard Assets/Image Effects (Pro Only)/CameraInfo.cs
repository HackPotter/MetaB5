// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
// pseudo image effect that displays useful info for your image effects



[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Camera Info")]

public class CameraInfo : MonoBehaviour
{

    // display current depth texture mode
    public DepthTextureMode currentDepthMode;
    // render path
    public RenderingPath currentRenderPath;
    // number of official image fx used
    public int currentPostFxCount = 0;

#if UNITY_EDITOR
    void Start()
    {
        UpdateInfo();
    }

    void Update()
    {
        if (currentDepthMode != GetComponent<Camera>().depthTextureMode)
            GetComponent<Camera>().depthTextureMode = currentDepthMode;
        if (currentRenderPath != GetComponent<Camera>().actualRenderingPath)
            GetComponent<Camera>().renderingPath = currentRenderPath;

        UpdateInfo();
    }

    void UpdateInfo()
    {
        currentDepthMode = GetComponent<Camera>().depthTextureMode;
        currentRenderPath = GetComponent<Camera>().actualRenderingPath;
        PostEffectsBase[] fx = gameObject.GetComponents<PostEffectsBase>();
        int fxCount = 0;
        foreach (PostEffectsBase post in fx)
            if (post.enabled)
                fxCount++;
        currentPostFxCount = fxCount;
    }
#endif
}

