// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;



[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Crease")]

public class Crease : PostEffectsBase
{
    public float intensity = 0.5f;
    public int softness = 1;
    public float spread = 1.0f;

    public Shader blurShader;
    private Material blurMaterial = null;

    public Shader depthFetchShader;
    private Material depthFetchMaterial = null;

    public Shader creaseApplyShader;
    private Material creaseApplyMaterial = null;

    void CreateMaterials()
    {
        blurMaterial = CheckShaderAndCreateMaterial(blurShader, blurMaterial);
        depthFetchMaterial = CheckShaderAndCreateMaterial(depthFetchShader, depthFetchMaterial);
        creaseApplyMaterial = CheckShaderAndCreateMaterial(creaseApplyShader, creaseApplyMaterial);
    }

    void Start()
    {
        CreateMaterials();
        CheckSupport(true);
    }

    void OnEnable()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        CreateMaterials();

        float widthOverHeight = (1.0f * source.width) / (1.0f * source.height);
        float oneOverBaseSize = 1.0f / 512.0f;

        RenderTexture hrTex = RenderTexture.GetTemporary(source.width, source.height, 0);
        RenderTexture lrTex1 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0);
        RenderTexture lrTex2 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0);

        Graphics.Blit(source, hrTex, depthFetchMaterial);
        Graphics.Blit(hrTex, lrTex1);

        for (int i = 0; i < softness; i++)
        {
            blurMaterial.SetVector("offsets", new Vector4(0.0f, spread * oneOverBaseSize, 0.0f, 0.0f));
            Graphics.Blit(lrTex1, lrTex2, blurMaterial);
            blurMaterial.SetVector("offsets", new Vector4(spread * oneOverBaseSize / widthOverHeight, 0.0f, 0.0f, 0.0f));
            Graphics.Blit(lrTex2, lrTex1, blurMaterial);
        }

        creaseApplyMaterial.SetTexture("_HrDepthTex", hrTex);
        creaseApplyMaterial.SetTexture("_LrDepthTex", lrTex1);
        creaseApplyMaterial.SetFloat("intensity", intensity);
        Graphics.Blit(source, destination, creaseApplyMaterial);

        RenderTexture.ReleaseTemporary(hrTex);
        RenderTexture.ReleaseTemporary(lrTex1);
        RenderTexture.ReleaseTemporary(lrTex2);
    }
}