using System.Collections;
using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
    public Camera mainCam;
    public Color mainCamBGColor;

    public GameObject mainLight;
   // public GameObject[] bouncedLight;

    public Material skybox;

    public bool day = true;
    public bool night;

    public bool animate;
	private Color curFogColor;
	private Color curLightColor;
    public float speed = 30.0f; //higher means slower transition
    public float speedOffset = 0.0f; //offset that can be added to specific attributes for even slower tranisiton speed
    public float animationDelay = 0.0f; //delay number in seconds for the time of day transition

    public float dayShadows = 0.4f;
    
    private float dayBlend = 1.0f;
    private float nightBlend = 0.0f;
    private float lerpBlend;

    public Color dayAmbientColor = new Color(0.27451f, 0.30588f, 0.32157f, 0);
    public Color dayFogColor = new Color(0.49804f, 0.55294f, 0.60392f, 0);
    public float dayFogDensity = 0.004f;
    public float nightShadows = 0.6f;

    public Color nightAmbientColor = new Color(0.07451f, 0.07059f, 0.14118f, 0);
    public Color nightFogColor = new Color(0.08235f, 0.05882f, 0.14510f, 0);
    public float nightFogDensity = 0.006f;

    static bool isDay;
    static bool isNight;

    static Color dayLightColor = Color.white;
    static float dayLightIntensity = 0.6f;
  //  private Color dayBouncedLightColor = dayLightColor;
   // private float dayBouncedLightIntensity = dayLightIntensity;

    public Color nightLightColor = new Color(0.0902f, 0.07451f, 0.2f, 0);
    static float nightLightIntensity = 0.25f;

   // private Color nightBouncedLightColor = nightLightColor;
   // private float nightBouncedLightIntensity = nightLightIntensity;

    void Start()
    {

        if (isDay == true)
        {
            day = true;
        }

        if (isNight == true)
        {
            night = true;
        }

        RenderSettings.skybox = skybox;
       // RenderSettings.skybox.SetFloat("_Blend", 0.0f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = UnityEngine.FogMode.ExponentialSquared;
		curFogColor = RenderSettings.fogColor;
		curLightColor = mainLight.GetComponent<Light>().color;

        mainLight.transform.position = new Vector3(0, 1000, 0);
        mainLight.transform.eulerAngles = new Vector3(50, 330, 0);



    }

    void Day()
    {

        RenderSettings.ambientLight = dayAmbientColor;
        RenderSettings.fogColor = dayFogColor;
        RenderSettings.fogDensity = dayFogDensity;
       // RenderSettings.skybox.SetFloat("_Blend", dayBlend);

        mainLight.GetComponent<Light>().color = dayLightColor;
        mainLight.GetComponent<Light>().intensity = dayLightIntensity;
        mainLight.GetComponent<Light>().shadowStrength = dayShadows;

      /*  foreach (GameObject gameObject in bouncedLight)
        {
            gameObject.light.color = dayLightColor;
            gameObject.light.intensity = dayLightIntensity;
        }*/

    }

    void Night()
    {

        RenderSettings.ambientLight = nightAmbientColor;
        RenderSettings.fogColor = nightFogColor;
        RenderSettings.fogDensity = nightFogDensity;
       // RenderSettings.skybox.SetFloat("_Blend", nightBlend);

        mainLight.GetComponent<Light>().color = nightLightColor;
        mainLight.GetComponent<Light>().intensity = nightLightIntensity;
        mainLight.GetComponent<Light>().shadowStrength = nightShadows;

         /*foreach (GameObject gameObject in bouncedLight)
        {
            gameObject.light.color = nightLightColor;
            gameObject.light.intensity = nightLightIntensity;
        }*/

    }

    IEnumerator Delay()
    {

        yield return new WaitForSeconds(animationDelay);

    }

    void Update()
    {

        //check if it's day time
        if (day == true)
        {

            Day();
            night = false;



            if (day & animate)
            {

                Delay();
                RenderSettings.ambientLight = Color.Lerp(dayAmbientColor, nightAmbientColor, (Time.time / speed));
                RenderSettings.fogColor = Color.Lerp(dayFogColor, nightFogColor, (Time.time / (speed + speedOffset)));
                RenderSettings.fogDensity = Mathf.Lerp(dayFogDensity, nightFogDensity, (Time.time / speed));
              //  RenderSettings.skybox.SetFloat("_Blend", lerpBlend);
                lerpBlend = Mathf.Lerp(dayBlend, nightBlend, (Time.time / speed));

                mainLight.GetComponent<Light>().color = Color.Lerp(dayLightColor, nightLightColor, (Time.time / speed));
                mainLight.GetComponent<Light>().intensity = Mathf.Lerp(dayLightIntensity, nightLightIntensity, (Time.time / speed));
                mainLight.GetComponent<Light>().shadowStrength = Mathf.Lerp(dayShadows, nightShadows, (Time.time / speed));

                /* foreach (GameObject gameObject in bouncedLight)
                {
                    gameObject.light.color = Color.Lerp(dayLightColor, nightLightColor, (Time.time / speed));
                    gameObject.light.intensity = Mathf.Lerp(dayLightIntensity, nightLightIntensity, (Time.time / speed));
                }*/

            }

        }
		
        //check if it's night time
        else if (night == true)
        {

            Night();
            day = false;



            if (night & animate)
            {

                Delay();
                RenderSettings.ambientLight = Color.Lerp(nightAmbientColor, dayAmbientColor, (Time.time / speed));
                RenderSettings.fogColor = Color.Lerp(curFogColor, dayFogColor, (Time.time / (speed + speedOffset)));
                RenderSettings.fogDensity = Mathf.Lerp(nightFogDensity, dayFogDensity, (Time.time / speed));
                //RenderSettings.skybox.SetFloat("_Blend", lerpBlend);
                lerpBlend = Mathf.Lerp(nightBlend, dayBlend, (Time.time / speed));

                mainLight.GetComponent<Light>().color = Color.Lerp(curLightColor, dayLightColor, (Time.time / speed));
                mainLight.GetComponent<Light>().intensity = Mathf.Lerp(nightLightIntensity, dayLightIntensity, (Time.time / speed));
                mainLight.GetComponent<Light>().shadowStrength = Mathf.Lerp(nightShadows, dayShadows, (Time.time / speed));

                /* foreach (GameObject gameObject in bouncedLight)
                {
                    gameObject.light.color = Color.Lerp(nightLightColor, dayLightColor, (Time.time / speed));
                    gameObject.light.intensity = Mathf.Lerp(nightLightIntensity, dayLightIntensity, (Time.time / speed));
                }*/

            }
        }
        mainCamBGColor = RenderSettings.fogColor;

        if (lerpBlend == dayBlend)
        {
            night = false;
            isDay = true;
        }

        if (lerpBlend == nightBlend)
        {
            day = false;
            isNight = true;
        }

    }
}