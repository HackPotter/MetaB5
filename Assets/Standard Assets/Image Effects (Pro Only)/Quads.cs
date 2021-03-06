// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

// same as Triangles but creates quads instead which generally
// saves fillrate at the expense for more triangles to issue
public class Quads : MonoBehaviour
{
    static Mesh[] meshes;
    static int currentQuads = 0;

    static bool HasMeshes()
    {
        if (meshes == null || meshes.Length == 0)
            return false;
        foreach (Mesh m in meshes)
            if (null == m)
                return false;

        return true;
    }

    public static void Cleanup()
    {
        if (meshes == null)
            return;

        foreach (Mesh m in meshes)
        {
            if (m != null)
            {
                DestroyImmediate(m);
            }
        }
        meshes = null;
    }

    public static Mesh[] GetMeshes(int totalWidth, int totalHeight)
    {
        if (HasMeshes() && (currentQuads == (totalWidth * totalHeight)))
        {
            return meshes;
        }

        int maxQuads = 65000 / 6;
        int totalQuads = totalWidth * totalHeight;
        currentQuads = totalQuads;

        int meshCount = Mathf.CeilToInt((1.0f * totalQuads) / (1.0f * maxQuads));

        meshes = new Mesh[meshCount];

        int i = 0;
        int index = 0;
        for (i = 0; i < totalQuads; i += maxQuads)
        {
            int quads = Mathf.FloorToInt(Mathf.Clamp((totalQuads - i), 0, maxQuads));

            meshes[index] = GetMesh(quads, i, totalWidth, totalHeight);
            index++;
        }

        return meshes;
    }

    static Mesh GetMesh(int triCount, int triOffset, int totalWidth, int totalHeight)
    {
        Mesh mesh = new Mesh();
        mesh.hideFlags = HideFlags.DontSave;

        Vector3[] verts = new Vector3[triCount * 4];
        Vector2[] uvs = new Vector2[triCount * 4];
        Vector2[] uvs2 = new Vector2[triCount * 4];
        int[] tris = new int[triCount * 6];


        for (int i = 0; i < triCount; i++)
        {
            int i4 = i * 4;
            int i6 = i * 6;

            int vertexWithOffset = triOffset + i;

            float x = Mathf.Floor(vertexWithOffset % totalWidth) / totalWidth;
            float y = Mathf.Floor(vertexWithOffset / totalWidth) / totalHeight;

            Vector3 position = new Vector3(x * 2 - 1, y * 2 - 1, 1.0f);

            verts[i4 + 0] = position;
            verts[i4 + 1] = position;
            verts[i4 + 2] = position;
            verts[i4 + 3] = position;

            uvs[i4 + 0] = new Vector2(0.0f, 0.0f);
            uvs[i4 + 1] = new Vector2(1.0f, 0.0f);
            uvs[i4 + 2] = new Vector2(0.0f, 1.0f);
            uvs[i4 + 3] = new Vector2(1.0f, 1.0f);

            uvs2[i4 + 0] = new Vector2(x, y);
            uvs2[i4 + 1] = new Vector2(x, y);
            uvs2[i4 + 2] = new Vector2(x, y);
            uvs2[i4 + 3] = new Vector2(x, y);

            tris[i6 + 0] = i4 + 0;
            tris[i6 + 1] = i4 + 1;
            tris[i6 + 2] = i4 + 2;

            tris[i6 + 3] = i4 + 1;
            tris[i6 + 4] = i4 + 2;
            tris[i6 + 5] = i4 + 3;

        }

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.uv2 = uvs2;

        return mesh;
    }
}
