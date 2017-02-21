using UnityEngine;

public class DestroyProteinShip : MonoBehaviour
{
    public Transform ship;
    public ParticleEmitter shipDebris;
    public ParticleEmitter[] ubiquitinParticleSystems;
    public float avoidDistance = 3.0f;
    public float lookAhead = 2.0f;
    public float maxSpeed = 5.0f;
    public float maxAcceleration = 10.0f;
    public float slowingRadius = 5.0f;
    public float targetRadius = 1.0f;
    public float timeToTarget = 0.25f;
    public GameObject myMainCam;

    private Vector3 myVel;
    private Vector3 initPos;
    private CameraChange cameraChange;
    private float vigCount;


    public bool destroyingShip = false;

    void Awake()
    {
        initPos = transform.position;
        cameraChange = transform.GetComponent<CameraChange>();
        vigCount = 2;
        transform.gameObject.AddComponent<Agent>();

    }


    void Update()
    {
        if (destroyingShip)
        {
            if (vigCount < 9)
            {
                vigCount += 0.2f;
                myMainCam.GetComponent<Vignetting>().intensity = vigCount;
            }

            transform.LookAt(ship);
            ship.LookAt(transform);

            transform.position += myVel * Time.deltaTime;

            Vector3 mv = getSteering();

            if (mv == Vector3.zero)
            {
                myVel = mv;
            }
            else
            {
                myVel += mv * Time.deltaTime;
            }

            if (myVel.magnitude > maxSpeed)
            {
                myVel.Normalize();
                myVel *= maxSpeed;
            }
        }
    }

    void DestroyShip()
    {
        //myMainCam.GetComponent<"Vignetting">().intensity = 10;
        destroyingShip = true;
    }

    void EmitShipDebris()
    {
        shipDebris.worldVelocity = transform.forward.normalized * 10.0f;
        shipDebris.emit = true;
    }

    void Reset()
    {
        iTween.CameraFadeTo(iTween.Hash("amount", 0.0f, "time", 1.0f, "delay", 0.0f, "onCompleteTarget", this.gameObject, "oncomplete", "Test"));
        vigCount = 2;
        myMainCam.GetComponent<Vignetting>().intensity = 1;
        transform.position = initPos;
        cameraChange.SetShipCam();
        shipDebris.emit = false;
        shipDebris.ClearParticles();
        for (int i = 0; i < ubiquitinParticleSystems.Length; i++)
        {
            ubiquitinParticleSystems[i].emit = true;
        }
    }


    Vector3 getSteering()
    {
        Vector3 steering;

        RaycastHit hitInfo;
        float offset = 0.2f;

        Vector3 p1 = transform.position + transform.forward * -3;
        Vector3 p2 = p1 + transform.forward * 6;
        float r = 1.4f + offset;

        if (Physics.CapsuleCast(p1, p2, r, transform.forward, out hitInfo, lookAhead) && !hitInfo.transform.Equals(GameObject.Find("Ship").transform))
        {
            Vector3 avoidTarget = hitInfo.point + hitInfo.normal * avoidDistance;

            steering = avoidTarget - transform.position;
            steering.Normalize();
            steering *= maxSpeed;

        }
        else
        {
            Vector3 desiredVel = ship.position - transform.position;

            if (desiredVel.magnitude < targetRadius)
            {
                cameraChange.SetProCam();
                destroyingShip = false;
                EmitShipDebris();
                iTween.CameraFadeTo(iTween.Hash("amount", 1.0f, "time", 2.0f, "delay", 3.0f, "onCompleteTarget", this.gameObject, "oncomplete", "Reset"));
                return Vector3.zero;
            }
            else if (desiredVel.magnitude > slowingRadius)
            {
                desiredVel.Normalize();
                desiredVel *= maxSpeed;
                steering = desiredVel - myVel;
            }
            else
            {
                float speed = maxSpeed * desiredVel.magnitude / slowingRadius;
                desiredVel.Normalize();
                desiredVel *= speed;

                steering = desiredVel - myVel;
                steering /= timeToTarget;

            }
        }
        
        if (steering.magnitude > maxAcceleration)
        {
            steering.Normalize();
            steering *= maxAcceleration;
        }

        return steering;
    }
}