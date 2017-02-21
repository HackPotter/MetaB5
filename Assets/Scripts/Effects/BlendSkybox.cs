using UnityEngine;

public class BlendSkybox : MonoBehaviour
{
    public float speed = 0.5f;
    //this is the speed your directional light rotates at
    //adjust this to your liking
    public Color colorEnd;
    public Color lerpedColor;

    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
        lerpedColor = Color.Lerp(lerpedColor, colorEnd, Time.time);
        RenderSettings.skybox.SetFloat("_Blend", (Time.deltaTime + speed) / 2.0f);
    }
}