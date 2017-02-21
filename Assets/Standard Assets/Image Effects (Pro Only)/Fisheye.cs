// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Fisheye")]

class Fisheye : PostEffectsBase
{
    public float strengthX = 0.05f;
    public float strengthY = 0.05f;

    public Shader fishEyeShader = null;
    private Material fisheyeMaterial = null;

    void CreateMaterials()
    {
        fisheyeMaterial = CheckShaderAndCreateMaterial(fishEyeShader, fisheyeMaterial);
    }

    void Start()
    {
        CreateMaterials();
        CheckSupport(false);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        CreateMaterials();

        float oneOverBaseSize = 80.0f / 512.0f; // to keep values more like in the old version of fisheye

        float ar = (source.width * 1.0f) / (source.height * 1.0f);

        fisheyeMaterial.SetVector("intensity", new Vector4(strengthX * ar * oneOverBaseSize, strengthY * oneOverBaseSize, strengthX * ar * oneOverBaseSize, strengthY * oneOverBaseSize));
        Graphics.Blit(source, destination, fisheyeMaterial);
    }
}
