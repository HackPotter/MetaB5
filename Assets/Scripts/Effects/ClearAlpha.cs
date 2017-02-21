using UnityEngine;

public class ClearAlpha : MonoBehaviour
{
    public float AlphaValue;
    private Material _material;

    void Start()
    {
        _material = new Material(
            @"Shader ""Hidden/Clear Alpha""
            {
                Properties
                {
                    _MainTex (""Base (RGB)"", 2D) = ""white"" {}
                    _Alpha(""Alpha"", Float)=1.0
                }
                SubShader
                {
                    Pass
                    {
                        ZTest Always Cull Off ZWrite Off
                        //ColorMask A

                        SetTexture [_MainTex]
                        {
                            constantColor (0,0,0,1)
                            combine constant + Texture
                            //constantColor(0,0,0,[_Alpha]) combine constant
                        }
                    }
                }
            }"
        );

        _material.SetFloat("_Alpha", 1);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        //Graphics.Blit(src, dst, _material);
        
        GL.PushMatrix();
        GL.LoadOrtho();
        //_material.SetFloat("_Alpha", AlphaValue);
        //_material.SetPass(0);
        Graphics.Blit(src, dst, _material);
        //GL.Begin(GL.QUADS);
        //GL.Vertex3(0, 0, 0.1f);
        //GL.Vertex3(1, 0, 0.1f);
        //GL.Vertex3(1, 1, 0.1f);
        //GL.Vertex3(0, 1, 0.1f);
        GL.End();
        GL.PopMatrix();
    }
}

