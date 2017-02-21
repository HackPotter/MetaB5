using UnityEngine;

public class FurForce : MonoBehaviour
{
    public float smoothing = 10;
    private Vector3 forceSmoothed;

    public bool addRigidbodyForce = false;
    public float rigidbodyForceFactor = 1;

    public bool addGravityToForce = false;
    public float gravityFactor = 0.1f;
    public bool addWindForce = false;
    public Vector3 windForceFactor = new Vector3(1, 1, 1);
    public float windForceSpeed = 1;

    private Vector3 perlinrandom;

    void Awake()
    {
        perlinrandom = new Vector3(Random.Range(0.0f, 65535.0f), Random.Range(0.0f, 65535.0f), Random.Range(0.0f, 65535.0f));
    }

    void Update()
    {
        Vector3 sumForce = Vector3.zero;
        
        //adding force based on rigidbody velocity
        if (addRigidbodyForce)
        {
            if (GetComponent<Rigidbody>())
            {
                sumForce = -GetComponent<Rigidbody>().velocity * rigidbodyForceFactor;
            }
        }

        //add gravity force
        if (addGravityToForce) sumForce += Physics.gravity * gravityFactor;

        //add wind force
        if (addWindForce) sumForce += Vector3.Scale(Noise(windForceSpeed), windForceFactor);

        //clamping force magnitude to 1.0f
        sumForce = Vector3.ClampMagnitude(sumForce, 1.0f);

        //check magnitude of both local and global vectors
        /*
        Debug.Log(sumForce + " " + localDir);
        FIXME_VAR_TYPE mag= Vector3.Magnitude(sumForce+localDir);
        if (mag>1) {
            sumForce = sumForce/mag;
            newLocalForce = localDir/mag;
        }
        if (mag==0) {
            sumForce = Vector3.zero;
            newLocalForce = Vector3.zero;
        }

        Debug.Log(mag + " " + sumForce + " " + newLocalForce);
        Debug.Log("--");
        */

        //smoothing force
        forceSmoothed = Vector3.Lerp(forceSmoothed, sumForce, Time.deltaTime * smoothing);
        //forceLocalSmoothed = Vector3.Lerp(forceLocalSmoothed, newLocalForce,Time.deltaTime * smoothing);

        //pass global force for fur
        GetComponent<Renderer>().material.SetVector("_ForceGlobal", forceSmoothed);
        //renderer.material.SetVector("_ForceLocal", forceLocalSmoothed);


    }

    //noise function - using to generate smooth wind
    Vector3 Noise(float speed)
    {
        float noise = Mathf.PerlinNoise(perlinrandom.x, Time.time * speed);
        float x = Mathf.Lerp(-1, 1, noise);
        float noise2 = Mathf.PerlinNoise(perlinrandom.y, Time.time * speed);
        float y = Mathf.Lerp(-1, 1, noise2);
        float noise3 = Mathf.PerlinNoise(perlinrandom.z, Time.time * speed);
        float z = Mathf.Lerp(-1, 1, noise3);

        return new Vector3(x, y, z);

    }
}