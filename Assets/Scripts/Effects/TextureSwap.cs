using UnityEngine;

// Oh god this is awful.
public class TextureSwap : MonoBehaviour
{
    public bool swapped = false;
    public string textureName = "_BumpMap";
    public Texture2D Tex1;
    public Texture2D Tex2;

    void Update()
    {

        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            if (swapped)
            {
                renderer.material.SetTexture(textureName, Tex2);
            }

            else
            {
                renderer.material.SetTexture(textureName, Tex1);
            }
        }

    }
}
