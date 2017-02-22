// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectsBase : MonoBehaviour
{
    protected Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
    {
        if (m2Create && m2Create.shader == s)
            return m2Create;

        if (!s)
        {
            Debug.Log("Missing shader in " + this.ToString());
            enabled = false;
            return null;
        }

        if (!s.isSupported)
        {
            NotSupported();
            Debug.LogError("The shader " + s.ToString() + " on effect " + this.ToString() + " is not supported on this platform!");
            return null;
        }
        else
        {
            m2Create = new Material(s);
            m2Create.hideFlags = HideFlags.DontSave;
            if (m2Create)
                return m2Create;
            else return null;
        }
    }

    Material CreateMaterial(Shader s, Material m2Create)
    {
        if (m2Create && m2Create.shader == s)
            return m2Create;

        if (!s)
        {
            Debug.Log("Missing shader in " + this.ToString());
            return null;
        }

        if (!s.isSupported)
        {
            return null;
        }
        else
        {
            m2Create = new Material(s);
            m2Create.hideFlags = HideFlags.DontSave;
            if (m2Create)
                return m2Create;
            else return null;
        }
    }

    // deprecated but needed for old effects to survive upgrading
    public bool CheckSupport()
    {
        return CheckSupport(false);
    }

    public bool CheckSupport(bool needDepth)
    {
        if (!SystemInfo.supportsImageEffects)
        {
            NotSupported();
            return false;
        }

        if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
        {
            NotSupported();
            return false;
        }

        return true;
    }

    // deprecated but needed for old effects to survive upgrading
    bool CheckShader(Shader s)
    {
        Debug.Log("The shader " + s.ToString() + " on effect " + this.ToString() + " is not part of the Unity 3.2f effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package.");
        if (!s.isSupported)
        {
            NotSupported();
            return false;
        }
        else
        {
            return false;
        }
    }

    void NotSupported()
    {
        Debug.LogError("The image effect " + this.ToString() + "is not supported on this platform!");
        enabled = false;
        return;
    }

    protected void DrawBorder(RenderTexture dest, Material material)
    {
        float x1;
        float x2;
        float y1;
        float y2;

        RenderTexture.active = dest;
        bool invertY = true; // source.texelSize.y < 0.0ff;
        // Set up the simple Matrix
        GL.PushMatrix();
        GL.LoadOrtho();

        for (int i = 0; i < material.passCount; i++)
        {
            material.SetPass(i);

            float y1_; float y2_;
            if (invertY)
            {
                y1_ = 1.0f; y2_ = 0.0f;
            }
            else
            {
                y1_ = 0.0f; y2_ = 1.0f;
            }

            // left	        
            x1 = 0.0f;
            x2 = 0.0f + 1.0f / (dest.width * 1.0f);
            y1 = 0.0f;
            y2 = 1.0f;
            GL.Begin(GL.QUADS);

            GL.TexCoord2(0.0f, y1_); GL.Vertex3(x1, y1, 0.1f);
            GL.TexCoord2(1.0f, y1_); GL.Vertex3(x2, y1, 0.1f);
            GL.TexCoord2(1.0f, y2_); GL.Vertex3(x2, y2, 0.1f);
            GL.TexCoord2(0.0f, y2_); GL.Vertex3(x1, y2, 0.1f);

            // right
            x1 = 1.0f - 1.0f / (dest.width * 1.0f);
            x2 = 1.0f;
            y1 = 0.0f;
            y2 = 1.0f;

            GL.TexCoord2(0.0f, y1_); GL.Vertex3(x1, y1, 0.1f);
            GL.TexCoord2(1.0f, y1_); GL.Vertex3(x2, y1, 0.1f);
            GL.TexCoord2(1.0f, y2_); GL.Vertex3(x2, y2, 0.1f);
            GL.TexCoord2(0.0f, y2_); GL.Vertex3(x1, y2, 0.1f);

            // top
            x1 = 0.0f;
            x2 = 1.0f;
            y1 = 0.0f;
            y2 = 0.0f + 1.0f / (dest.height * 1.0f);

            GL.TexCoord2(0.0f, y1_); GL.Vertex3(x1, y1, 0.1f);
            GL.TexCoord2(1.0f, y1_); GL.Vertex3(x2, y1, 0.1f);
            GL.TexCoord2(1.0f, y2_); GL.Vertex3(x2, y2, 0.1f);
            GL.TexCoord2(0.0f, y2_); GL.Vertex3(x1, y2, 0.1f);

            // bottom
            x1 = 0.0f;
            x2 = 1.0f;
            y1 = 1.0f - 1.0f / (dest.height * 1.0f);
            y2 = 1.0f;

            GL.TexCoord2(0.0f, y1_); GL.Vertex3(x1, y1, 0.1f);
            GL.TexCoord2(1.0f, y1_); GL.Vertex3(x2, y1, 0.1f);
            GL.TexCoord2(1.0f, y2_); GL.Vertex3(x2, y2, 0.1f);
            GL.TexCoord2(0.0f, y2_); GL.Vertex3(x1, y2, 0.1f);

            GL.End();
        }

        GL.PopMatrix();
    }
}