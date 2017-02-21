using UnityEngine;

public class ScrollingUVs : MonoBehaviour
{
    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public string textureName = "_MainTex";

    Vector2 uvOffset = Vector2.zero;

    void LateUpdate()
    {
        // Find every renderer in the object's children and tell them to scroll UVs
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            uvOffset += (uvAnimationRate * Time.deltaTime);
            if (renderer.enabled)
            {
                renderer.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
            }
        }


    }
}
