using UnityEngine;

public class TriggerFogColor : MonoBehaviour
{
    private Color exteriorFogColor;
    private Color interiorFogColor;

    private float exteriorFogDensity;
    private float interiorFogDensity;

    private Color exteriorAmbientColor;
    private Color interiorAmbientColor;

    private bool isInside;
    private bool isOutside;

    public float offsetValue = 0.002f;
    public Color tintColor = Color.grey;
    public float lerpSpeed = 1.0f; //seconds

    void Start()
    {
        //Set the background color, main fog color and fog density
        exteriorFogColor = RenderSettings.fogColor;
        exteriorFogDensity = RenderSettings.fogDensity;
        exteriorAmbientColor = RenderSettings.ambientLight;

        interiorFogColor = exteriorFogColor * tintColor;
        interiorFogDensity = exteriorFogDensity + offsetValue;
        interiorAmbientColor = exteriorAmbientColor * tintColor;
    }

    void Reset()
    {
        isOutside = false;
        isInside = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FogTrigger")
        {
            isInside = true;
            isOutside = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "FogTrigger")
        {
            isOutside = true;
            isInside = false;
        }
    }

    void Update()
    {
        if (isInside == true)
        {
            RenderSettings.fogColor = Color.Lerp(exteriorFogColor, interiorFogColor, (Time.time * lerpSpeed));
            RenderSettings.fogDensity = Mathf.Lerp(exteriorFogDensity, interiorFogDensity, (Time.time * lerpSpeed));
            RenderSettings.ambientLight = Color.Lerp(exteriorAmbientColor, interiorAmbientColor, (Time.time * lerpSpeed));

            Reset();
        }

        if (isOutside == true)
        {
            RenderSettings.fogColor = Color.Lerp(interiorFogColor, exteriorFogColor, (Time.time * lerpSpeed));
            RenderSettings.fogDensity = Mathf.Lerp(interiorFogDensity, exteriorFogDensity, (Time.time * lerpSpeed));
            RenderSettings.ambientLight = Color.Lerp(interiorAmbientColor, exteriorAmbientColor, (Time.time * lerpSpeed));

            Reset();
        }
    }
}