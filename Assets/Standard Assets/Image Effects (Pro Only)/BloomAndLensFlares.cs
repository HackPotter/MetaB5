// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;




public enum LensflareStyle34
{
    Ghosting = 0,
    Anamorphic = 1,
    Combined = 2,
}

public enum TweakMode34
{
    Basic = 0,
    Complex = 1,
}

public enum BloomScreenBlendMode
{
    Screen = 0,
    Add = 1,
}

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Bloom and Lens Flares (3.4f)")]
public class BloomAndLensFlares : PostEffectsBase
{
    public TweakMode34 tweakMode = 0;
    public BloomScreenBlendMode screenBlendMode = BloomScreenBlendMode.Screen;

    public float sepBlurSpread = 1.5f;
    public float useSrcAlphaAsMask = 0.5f;

    public float bloomIntensity = 1.0f;
    public float bloomThreshhold = 0.5f;
    public int bloomBlurIterations = 2;

    public bool lensflares = false;

    public int hollywoodFlareBlurIterations = 2;
    public LensflareStyle34 lensflareMode = LensflareStyle34.Anamorphic;
    public float hollyStretchWidth = 3.5f;
    public float lensflareIntensity = 1.0f;
    public float lensflareThreshhold = 0.3f;
    public Color flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);
    public Color flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);
    public Color flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);
    public Color flareColorD = new Color(0.8f, 0.4f, 0.0f, 0.75f);
    public float blurWidth = 1.0f;

    public Shader lensFlareShader;
    private Material lensFlareMaterial;

    public Shader vignetteShader;
    private Material vignetteMaterial;

    public Shader separableBlurShader;
    private Material separableBlurMaterial;

    public Shader addBrightStuffOneOneShader;
    private Material addBrightStuffBlendOneOneMaterial;

    public Shader screenBlendShader;
    private Material screenBlend;

    public Shader hollywoodFlaresShader;
    private Material hollywoodFlaresMaterial;

    public Shader brightPassFilterShader;
    private Material brightPassFilterMaterial;


    void Start()
    {
        CreateMaterials();
        CheckSupport(false);
    }

    void CreateMaterials()
    {
        screenBlend = CheckShaderAndCreateMaterial(screenBlendShader, screenBlend);
        lensFlareMaterial = CheckShaderAndCreateMaterial(lensFlareShader, lensFlareMaterial);
        vignetteMaterial = CheckShaderAndCreateMaterial(vignetteShader, vignetteMaterial);
        separableBlurMaterial = CheckShaderAndCreateMaterial(separableBlurShader, separableBlurMaterial);
        addBrightStuffBlendOneOneMaterial = CheckShaderAndCreateMaterial(addBrightStuffOneOneShader, addBrightStuffBlendOneOneMaterial);
        hollywoodFlaresMaterial = CheckShaderAndCreateMaterial(hollywoodFlaresShader, hollywoodFlaresMaterial);
        brightPassFilterMaterial = CheckShaderAndCreateMaterial(brightPassFilterShader, brightPassFilterMaterial);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        CreateMaterials();

        RenderTexture halfRezColor = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0);
        RenderTexture quarterRezColor = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
        RenderTexture secondQuarterRezColor = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
        RenderTexture thirdQuarterRezColor = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);

        float widthOverHeight = (1.0f * source.width) / (1.0f * source.height);
        float oneOverBaseSize = 1.0f / 512.0f;

        // downsample

        Graphics.Blit(source, halfRezColor, screenBlend, 2); // <- stable downsample
        Graphics.Blit(halfRezColor, quarterRezColor, screenBlend, 2); // <- stable downsample	

        RenderTexture.ReleaseTemporary(halfRezColor);

        // cut colors (threshholding)			

        BrightFilter(bloomThreshhold, useSrcAlphaAsMask, quarterRezColor, secondQuarterRezColor);

        // blurring

        if (bloomBlurIterations < 1)
            bloomBlurIterations = 1;

        for (int iter = 0; iter < bloomBlurIterations; iter++)
        {
            float spreadForPass = (bloomBlurIterations * 1.0f) * sepBlurSpread;
            separableBlurMaterial.SetVector("offsets", new Vector4(0.0f, spreadForPass * oneOverBaseSize, 0.0f, 0.0f));
            Graphics.Blit(iter == 0 ? secondQuarterRezColor : quarterRezColor, thirdQuarterRezColor, separableBlurMaterial);
            separableBlurMaterial.SetVector("offsets", new Vector4((spreadForPass / widthOverHeight) * oneOverBaseSize, 0.0f, 0.0f, 0.0f));
            Graphics.Blit(thirdQuarterRezColor, quarterRezColor, separableBlurMaterial);
        }

        if (lensflares)
        {

            // this effect supports different kind of lens flares: ghosting, anamorphic and combined

            // (a) ghosting?

            if (lensflareMode == 0)
            {

                // cut off some more dark colors

                BrightFilter(lensflareThreshhold, 0.0f, secondQuarterRezColor, thirdQuarterRezColor);

                // smooth a little, this needs to be resolution dependent

                separableBlurMaterial.SetVector("offsets", new Vector4(0.0f, (2.0f) / (1.0f * quarterRezColor.height), 0.0f, 0.0f));
                Graphics.Blit(thirdQuarterRezColor, secondQuarterRezColor, separableBlurMaterial);
                separableBlurMaterial.SetVector("offsets", new Vector4((2.0f) / (1.0f * quarterRezColor.width), 0.0f, 0.0f, 0.0f));
                Graphics.Blit(secondQuarterRezColor, thirdQuarterRezColor, separableBlurMaterial);

                // no ugly edges!

                Vignette(0.975f, thirdQuarterRezColor, secondQuarterRezColor);

                BlendFlares(secondQuarterRezColor, quarterRezColor);
            }

            // (b) hollywood/anamorphic flares?

            else
            {

                // thirdQuarter has the brightcut unblurred colors
                // quarterRezColor is the blurred, brightcut buffer that will end up as bloom

                hollywoodFlaresMaterial.SetVector("_Threshhold", new Vector4(lensflareThreshhold, 1.0f / (1.0f - lensflareThreshhold), 0.0f, 0.0f));
                hollywoodFlaresMaterial.SetVector("tintColor", new Vector4(flareColorA.r, flareColorA.g, flareColorA.b, flareColorA.a) * flareColorA.a * lensflareIntensity);
                Graphics.Blit(thirdQuarterRezColor, secondQuarterRezColor, hollywoodFlaresMaterial, 2);
                Graphics.Blit(secondQuarterRezColor, thirdQuarterRezColor, hollywoodFlaresMaterial, 3);

                hollywoodFlaresMaterial.SetVector("offsets", new Vector4((sepBlurSpread * 1.0f / widthOverHeight) * oneOverBaseSize, 0.0f, 0.0f, 0.0f));
                hollywoodFlaresMaterial.SetFloat("stretchWidth", hollyStretchWidth);
                Graphics.Blit(thirdQuarterRezColor, secondQuarterRezColor, hollywoodFlaresMaterial, 1);
                hollywoodFlaresMaterial.SetFloat("stretchWidth", hollyStretchWidth * 2.0f);
                Graphics.Blit(secondQuarterRezColor, thirdQuarterRezColor, hollywoodFlaresMaterial, 1);
                hollywoodFlaresMaterial.SetFloat("stretchWidth", hollyStretchWidth * 4.0f);
                Graphics.Blit(thirdQuarterRezColor, secondQuarterRezColor, hollywoodFlaresMaterial, 1);

                if (lensflareMode == (LensflareStyle34)1)
                {
                    for (int itera = 0; itera < hollywoodFlareBlurIterations; itera++)
                    {
                        separableBlurMaterial.SetVector("offsets", new Vector4((hollyStretchWidth * 2.0f / widthOverHeight) * oneOverBaseSize, 0.0f, 0.0f, 0.0f));
                        Graphics.Blit(secondQuarterRezColor, thirdQuarterRezColor, separableBlurMaterial);
                        separableBlurMaterial.SetVector("offsets", new Vector4((hollyStretchWidth * 2.0f / widthOverHeight) * oneOverBaseSize, 0.0f, 0.0f, 0.0f));
                        Graphics.Blit(thirdQuarterRezColor, secondQuarterRezColor, separableBlurMaterial);
                    }

                    AddTo(1.0f, secondQuarterRezColor, quarterRezColor);
                }
                else
                {

                    // (c) combined?

                    for (int ix = 0; ix < hollywoodFlareBlurIterations; ix++)
                    {
                        separableBlurMaterial.SetVector("offsets", new Vector4((hollyStretchWidth * 2.0f / widthOverHeight) * oneOverBaseSize, 0.0f, 0.0f, 0.0f));
                        Graphics.Blit(secondQuarterRezColor, thirdQuarterRezColor, separableBlurMaterial);
                        separableBlurMaterial.SetVector("offsets", new Vector4((hollyStretchWidth * 2.0f / widthOverHeight) * oneOverBaseSize, 0.0f, 0.0f, 0.0f));
                        Graphics.Blit(thirdQuarterRezColor, secondQuarterRezColor, separableBlurMaterial);
                    }

                    Vignette(1.0f, secondQuarterRezColor, thirdQuarterRezColor);

                    BlendFlares(thirdQuarterRezColor, secondQuarterRezColor);

                    AddTo(1.0f, secondQuarterRezColor, quarterRezColor);
                }
            }
        }

        // screen blend bloom results to color buffer

        screenBlend.SetFloat("_Intensity", bloomIntensity);
        screenBlend.SetTexture("_ColorBuffer", source);
        Graphics.Blit(quarterRezColor, destination, screenBlend, (int)screenBlendMode);

        RenderTexture.ReleaseTemporary(quarterRezColor);
        RenderTexture.ReleaseTemporary(secondQuarterRezColor);
        RenderTexture.ReleaseTemporary(thirdQuarterRezColor);
    }

    private void AddTo(float intensity, RenderTexture from, RenderTexture to)
    {
        addBrightStuffBlendOneOneMaterial.SetFloat("intensity", intensity);
        Graphics.Blit(from, to, addBrightStuffBlendOneOneMaterial);
    }

    private void BlendFlares(RenderTexture from, RenderTexture to)
    {
        lensFlareMaterial.SetVector("colorA", new Vector4(flareColorA.r, flareColorA.g, flareColorA.b, flareColorA.a) * lensflareIntensity);
        lensFlareMaterial.SetVector("colorB", new Vector4(flareColorB.r, flareColorB.g, flareColorB.b, flareColorB.a) * lensflareIntensity);
        lensFlareMaterial.SetVector("colorC", new Vector4(flareColorC.r, flareColorC.g, flareColorC.b, flareColorC.a) * lensflareIntensity);
        lensFlareMaterial.SetVector("colorD", new Vector4(flareColorD.r, flareColorD.g, flareColorD.b, flareColorD.a) * lensflareIntensity);
        Graphics.Blit(from, to, lensFlareMaterial);
    }

    private void BrightFilter(float thresh, float useAlphaAsMask, RenderTexture from, RenderTexture to)
    {
        brightPassFilterMaterial.SetVector("threshhold", new Vector4(thresh, 1.0f / (1.0f - thresh), 0.0f, 0.0f));
        brightPassFilterMaterial.SetFloat("useSrcAlphaAsMask", useAlphaAsMask);
        Graphics.Blit(from, to, brightPassFilterMaterial);
    }

    private void Vignette(float amount, RenderTexture from, RenderTexture to)
    {
        vignetteMaterial.SetFloat("vignetteIntensity", amount);
        Graphics.Blit(from, to, vignetteMaterial);
    }

}