// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;





public enum AAMode
{
    FXAA2 = 0,
    FXAA1PresetA = 1,
    FXAA1PresetB = 2,
    NFAA = 3,
    SSAA = 4,
    DLAA = 5,
}

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Antialiasing (Image based)")]
public class AntialiasingAsPostEffect : PostEffectsBase
{
    public AAMode mode = AAMode.FXAA2;

    public bool showGeneratedNormals = false;
    public float offsetScale = 0.2f;
    public float blurRadius = 18.0f;

    public bool dlaaSharp = false;

    public Shader ssaaShader;
    private Material ssaa;
    public Shader dlaaShader;
    private Material dlaa;
    public Shader nfaaShader;
    private Material nfaa;
    public Shader shaderFXAAPreset2;
    private Material materialFXAAPreset2;
    public Shader shaderFXAAPreset3;
    private Material materialFXAAPreset3;
    public Shader shaderFXAAII;
    private Material materialFXAAII;

    void CreateMaterials()
    {
        materialFXAAPreset2 = CheckShaderAndCreateMaterial(shaderFXAAPreset2, materialFXAAPreset2);
        materialFXAAPreset3 = CheckShaderAndCreateMaterial(shaderFXAAPreset3, materialFXAAPreset3);
        materialFXAAII = CheckShaderAndCreateMaterial(shaderFXAAII, materialFXAAII);
        nfaa = CheckShaderAndCreateMaterial(nfaaShader, nfaa);
        ssaa = CheckShaderAndCreateMaterial(ssaaShader, ssaa);
        dlaa = CheckShaderAndCreateMaterial(dlaaShader, dlaa);
    }

    void Start()
    {
        CreateMaterials();
        CheckSupport(false);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        CreateMaterials();

        if (mode < AAMode.NFAA)
        {

            // .............................................................................
            // FXAA antialiasing modes .....................................................			

            Material mat;
            if (mode == AAMode.FXAA1PresetB)
                mat = materialFXAAPreset3;
            else if (mode == AAMode.FXAA1PresetA)
                mat = materialFXAAPreset2;
            else
                mat = materialFXAAII;

            if (mode == AAMode.FXAA1PresetA)
                source.anisoLevel = 4;
            Graphics.Blit(source, destination, mat);
            if (mode == AAMode.FXAA1PresetA)
                source.anisoLevel = 0;
        }
        else if (mode == AAMode.SSAA)
        {

            // .............................................................................
            // SSAA antialiasing ...........................................................

            Graphics.Blit(source, destination, ssaa);
        }
        else if (mode == AAMode.DLAA)
        {

            // .............................................................................
            // DLAA antialiasing ...........................................................

            source.anisoLevel = 0;
            RenderTexture interim = RenderTexture.GetTemporary(source.width, source.height);
            Graphics.Blit(source, interim, dlaa, 0);
            Graphics.Blit(interim, destination, dlaa, dlaaSharp ? 2 : 1);
            RenderTexture.ReleaseTemporary(interim);
        }
        else if (mode == AAMode.NFAA)
        {

            // .............................................................................
            // nfaa antialiasing ..............................................

            source.anisoLevel = 0;

            nfaa.SetFloat("_OffsetScale", offsetScale);
            nfaa.SetFloat("_BlurRadius", blurRadius);

            Graphics.Blit(source, destination, nfaa, showGeneratedNormals ? 1 : 0);
        }
        else
        {

            Graphics.Blit(source, destination);
        }
    }
}