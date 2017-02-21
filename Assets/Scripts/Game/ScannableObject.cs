using UnityEngine;

public class ScannableObject : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _biologEntry;
    [SerializeField]
    private Color defaultColor;

    public float lerpDuration = (1.0f);
#pragma warning restore 0067, 0649

    public string BiologEntry
    {
        get { return _biologEntry; }
    }

    private static readonly Color kScanColor = new Color((122.0f / 255.0f), (199.0f / 255.0f), 1, 1);

    private Vector2 uvOffset = Vector2.zero;
	private Color curColor;
	private float lerp;
    
    void Start()
    {
        enabled = false;
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
			if (!renderer.material.HasProperty("_RimColor"))
			{
				//DebugFormatter.LogError(renderer.gameObject, "{0} must have property \"_RimColor\" for ScannableObject to work correctly", renderer.material.name);
				continue;
			}
            renderer.material.SetColor("_RimColor", defaultColor);
        }
    }

    void OnDisable()
    {
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            if (renderer.GetComponent<ParticleSystem>() || renderer is ParticleRenderer)
            {
                continue;
            }
			if (!renderer.material.HasProperty("_RimColor"))
			{
				//DebugFormatter.LogError(renderer.gameObject, "{0} must have property \"_RimColor\" for ScannableObject to work correctly", renderer.material.name);
				continue;
			}
			curColor = renderer.material.GetColor("_RimColor");
            renderer.material.SetColor("_RimColor", defaultColor);
        }
    }

    void Update()
    {
		lerp = Mathf.PingPong (Time.time, lerpDuration*0.5f) / (lerpDuration * 0.5f);
		
		foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
			if (!renderer.material.HasProperty("_RimColor"))
			{
				//DebugFormatter.LogError(renderer.gameObject, "{0} must have property \"_RimColor\" for ScannableObject to work correctly", renderer.material.name);
				continue;
			}
            if (renderer.GetComponent<ParticleSystem>() || renderer is ParticleRenderer)
            {
                continue;
            }
			renderer.material.SetColor("_RimColor", Color.Lerp(defaultColor, kScanColor, lerp));
        }
    }
}
