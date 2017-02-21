using System.Collections;
using UnityEngine;

public class PulseMaterialColor : MonoBehaviour
{
    public GameObject target;
    public Color color0 = Color.magenta;
    public Color color1 = Color.red;
    public float duration = 1.0f;

    IEnumerator Start()
    {
        while (true)
        {
            yield return LerpLightColor (color0, Color.red);
            yield return LerpLightColor (Color.red, color1);
            yield return LerpLightColor (color1, Color.red);
            yield return LerpLightColor (Color.red, color0);
        }
    }

    IEnumerator LerpLightColor(Color col1, Color col2)
    {
        float t = 0.0f;
        float rate = 1.0f / duration;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            target.GetComponent<Renderer>().material.color = Color.Lerp(col1, col2, t);
            yield return 0;
        }
    }
}