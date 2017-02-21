using UnityEngine;

public class AnimateUVs : MonoBehaviour
{
    public string TextureName = "_Detail";
    public string TextureBumpName = "_DetailBump";

    public bool AnimateBumpTexture = true;

    public bool u = false;
    public bool v = false;
    public float speed = 0.1f;
    public float min = -0.01f;
    public float max = 0.01f;

    void Update()
    {
        float offset = speed * (Time.time * Mathf.PingPong(Random.Range(min, max), Random.Range(min, max)));
        if (u == true && v == true)
        {
            GetComponent<Renderer>().material.SetTextureOffset(TextureName, new Vector2(offset, offset));
            if (AnimateBumpTexture)
            {
                GetComponent<Renderer>().material.SetTextureOffset(TextureBumpName, new Vector2(offset, offset));
            }
        }

        else if (u == true)
        {
            GetComponent<Renderer>().material.SetTextureOffset(TextureName, new Vector2(offset, 0));
            if (AnimateBumpTexture)
            {
                GetComponent<Renderer>().material.SetTextureOffset(TextureBumpName, new Vector2(offset, 0));
            }
        }

        else if (v == true)
        {
            GetComponent<Renderer>().material.SetTextureOffset(TextureName, new Vector2(0, offset));
            if (AnimateBumpTexture)
            {
                GetComponent<Renderer>().material.SetTextureOffset(TextureBumpName, new Vector2(0, offset));
            }
        }

    }
}