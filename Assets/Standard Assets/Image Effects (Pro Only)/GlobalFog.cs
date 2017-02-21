// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public enum FogMode
{
    AbsoluteYAndDistance = 0,
    AbsoluteY = 1,
    Distance = 2,
    RelativeYAndDistance = 3,
}

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Global Fog")]
public class GlobalFog : PostEffectsBase
{
    public FogMode fogMode = FogMode.AbsoluteYAndDistance;

    private float CAMERA_NEAR = 0.5f;
    private float CAMERA_FAR = 50.0f;
    private float CAMERA_FOV = 60.0f;
    private float CAMERA_ASPECT_RATIO = 1.333333f;

    public float startDistance = 200.0f;
    public float globalDensity = 1.0f;
    public float heightScale = 100.0f;
    public float height = 0.0f;

    public Color globalFogColor = Color.grey;

    public Shader fogShader;
    private Material fogMaterial = null;

    void CreateMaterials()
    {
        fogMaterial = CheckShaderAndCreateMaterial(fogShader, fogMaterial);
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

        bool updateFromCamera = true;
        if (updateFromCamera)
        {
            CAMERA_NEAR = GetComponent<Camera>().nearClipPlane;
            CAMERA_FAR = GetComponent<Camera>().farClipPlane;
            CAMERA_FOV = GetComponent<Camera>().fieldOfView;
            CAMERA_ASPECT_RATIO = GetComponent<Camera>().aspect;
        }

        Matrix4x4 frustumCorners = Matrix4x4.identity;

        float fovWHalf = CAMERA_FOV * 0.5f;

        Vector3 toRight = GetComponent<Camera>().transform.right * CAMERA_NEAR * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * CAMERA_ASPECT_RATIO;
        Vector3 toTop = GetComponent<Camera>().transform.up * CAMERA_NEAR * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = (GetComponent<Camera>().transform.forward * CAMERA_NEAR - toRight + toTop);
        float CAMERA_SCALE = topLeft.magnitude * CAMERA_FAR / CAMERA_NEAR;

        topLeft.Normalize();
        topLeft *= CAMERA_SCALE;

        Vector3 topRight = (GetComponent<Camera>().transform.forward * CAMERA_NEAR + toRight + toTop);
        topRight.Normalize();
        topRight *= CAMERA_SCALE;

        Vector3 bottomRight = (GetComponent<Camera>().transform.forward * CAMERA_NEAR + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= CAMERA_SCALE;

        Vector3 bottomLeft = (GetComponent<Camera>().transform.forward * CAMERA_NEAR - toRight - toTop);
        bottomLeft.Normalize();
        bottomLeft *= CAMERA_SCALE;

        frustumCorners.SetRow(0, topLeft);
        frustumCorners.SetRow(1, topRight);
        frustumCorners.SetRow(2, bottomRight);
        frustumCorners.SetRow(3, bottomLeft);

        fogMaterial.SetMatrix("_FrustumCornersWS", frustumCorners);
        fogMaterial.SetVector("_CameraWS", GetComponent<Camera>().transform.position);
        fogMaterial.SetVector("_StartDistance", new Vector4(1.0f / startDistance, (CAMERA_SCALE - startDistance)));
        fogMaterial.SetVector("_Y", new Vector4(height, 1.0f / heightScale));

        fogMaterial.SetFloat("_GlobalDensity", globalDensity * 0.01f);
        fogMaterial.SetColor("_FogColor", globalFogColor);

        CustomGraphicsBlit(source, destination, fogMaterial, (int)fogMode);
    }

    static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
    {
        RenderTexture.active = dest;

        fxMaterial.SetTexture("_MainTex", source);

        // bool  invertY = source.texelSize.y < 0.0ff;

        GL.PushMatrix();
        GL.LoadOrtho();

        fxMaterial.SetPass(passNr);

        GL.Begin(GL.QUADS);

        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 3.0f); // BL

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 2.0f); // BR

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 1.0f); // TR

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f); // TL

        GL.End();
        GL.PopMatrix();
    }
}