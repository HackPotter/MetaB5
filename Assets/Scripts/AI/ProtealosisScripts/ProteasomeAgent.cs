using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Metablast/Protealosis/Proteasome Agent")]
[RequireComponent(typeof(Rigidbody))]
public class ProteasomeAgent : Agent {

    public static event Action ProtealosisBeginning;
    public static event Action Protealyzed;
    public Tether tether;
    public Wander wander;
    public Pursue pursue;

    private Substrate _substrate;

    private static List<ProteasomeAgent> _allProteasomes = new List<ProteasomeAgent>();

    private static bool _protealizingCutscene = false;
    private static ProteasomeAgent _pursuingAgent = null;

    private AudioSource alarmSource;
    public AudioClip AlarmSound;
    public float AlarmCooldown = 2;
    private bool _protealizing = false;
    public bool _isPursuing = false;
    public float _pursuitAcceleration = 0.5f;
    public float _pursuitSpeed = 2.0f;
    public float _wanderAcceleration = 2.0f;
    public float _wanderSpeed = 0.5f;
    public float _maximumDistanceToPursue = 50f;

    private Vector3 startingPoint;
    private Quaternion startingRotation;

    void Awake() {
        _allProteasomes.Add(this);
        startingPoint = transform.position;
        startingRotation = transform.rotation;

        tether.SetTetherRadius(400);

        wander.enabled = true;
        pursue.enabled = false;
        tether.enabled = false;
    }

    void OnDestroy() {
        _allProteasomes.Remove(this);
        _pursuingAgent = null;
    }

    void Start() {
        _substrate = Substrate.Instance;
        alarmSource = gameObject.AddComponent<AudioSource>();
        alarmSource.clip = AlarmSound;
        alarmSource.pitch = 0.37f;
        StartCoroutine(AlarmSoundRoutine());
    }

    protected override void FixedUpdate() {
        ResolveCurrentBehavior();
        base.FixedUpdate();
    }

    void ResolveCurrentBehavior() {
        if (_protealizingCutscene) {
            wander.enabled = true;
            pursue.enabled = false;
            tether.enabled = false;
            MaxSpeed = _wanderSpeed;
            MaxAcceleration = _wanderAcceleration;
            _pursuingAgent = null;
            return;
        }
        if (_protealizing) {
            wander.enabled = false;
            pursue.enabled = false;
            tether.enabled = false;
        }
        if (_substrate.CanBeProtealized) {
            ProteasomeAgent closestAgent = null;
            foreach (ProteasomeAgent agent in _allProteasomes) {
                if (closestAgent == null ||
                    (Vector3.Distance(agent.transform.position, pursue.target.transform.position) < Vector3.Distance(closestAgent.transform.position, pursue.target.transform.position)) &&
                     Vector3.Distance(agent.transform.position, pursue.target.transform.position) < _maximumDistanceToPursue) {
                    closestAgent = agent;
                }
            }
            // If nobody is pursuing and we can pursue, then pursue.
            if (_pursuingAgent == null && Vector3.Distance(transform.position, pursue.target.transform.position) < _maximumDistanceToPursue) {
                MaxSpeed = _pursuitSpeed;
                MaxAcceleration = _pursuitAcceleration;
                _pursuingAgent = this;
                pursue.enabled = true;
                tether.enabled = false;
                wander.enabled = false;
            }
            // Give up if we're pursuing but too far away.
            else if (_pursuingAgent == this && Vector3.Distance(transform.position, pursue.target.transform.position) >= _maximumDistanceToPursue) {
                wander.enabled = true;
                tether.enabled = false;
                pursue.enabled = false;
                MaxSpeed = _wanderSpeed;
                MaxAcceleration = _wanderAcceleration;
                _pursuingAgent = null;
            }
        }
        // If we can't protealize, don't pursue.
        else {
            wander.enabled = true;
            pursue.enabled = false;
            tether.enabled = false;
            MaxSpeed = _wanderSpeed;
            MaxAcceleration = _wanderAcceleration;
            _pursuingAgent = null;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (_substrate.CanBeProtealized && collision.gameObject.tag == "Player") {
            if (!_protealizingCutscene) {
                StartCoroutine(ShipProtealizationRoutine(collision.gameObject));
            }
        }
    }

    IEnumerator AlarmSoundRoutine() {
        while (true) {
            if (_pursuingAgent == this) {
                alarmSource.Play();
            }
            yield return new WaitForSeconds(AlarmCooldown);
        }
    }

    IEnumerator ShipProtealizationRoutine(GameObject ship) {
        VehicleController3D shipControls = ship.GetComponent<VehicleController3D>();
        shipControls.enabled = false;
        ship.GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().isKinematic = true;
        _protealizingCutscene = true;
        _protealizing = true;

        tether.enabled = false;
        pursue.enabled = false;
        wander.enabled = false;

        Quaternion startProteasomeRotation = transform.rotation;
        Quaternion targetProteasomeRotation = Quaternion.LookRotation(ship.transform.position - this.transform.position);

        Quaternion startShipRotation = ship.transform.rotation;
        Quaternion targetShipRotation = Quaternion.LookRotation(transform.position - ship.transform.position);

        if (ProtealosisBeginning != null) {
            ProtealosisBeginning();
        }
        float start = Time.time;
        while (Time.time - start <= 1.5f) {
            transform.rotation = Quaternion.Slerp(startProteasomeRotation, targetProteasomeRotation, (Time.time - start) / 1.5f);
            ship.transform.rotation = Quaternion.Slerp(startShipRotation, targetShipRotation, (Time.time - start) / 1.5f);
            yield return null;
        }

        transform.rotation = targetProteasomeRotation;
        ship.transform.rotation = targetShipRotation;

        Vector3 startPosition = ship.transform.position;
        start = Time.time;
        while (Time.time - start <= 1.5f) {
            ship.transform.position = Vector3.Lerp(startPosition, transform.position, (Time.time - start) / 1.5f);
            yield return null;
        }

        // Brad 3/6/2017
        ParticleSystem shipDebris = this.transform.FindChild("Particle System").GetComponent<ParticleSystem>();
        shipDebris.Play(); // Begin animation and emission of particles when ship is caught by Proteasome

        yield return new WaitForSeconds(5.0f);

        CameraFade.Instance.StartFade(new Color(0, 0, 0), 1.0f);
        //iTween.CameraFadeTo(iTween.Hash("amount", 1.0f, "time", 1.0f, "delay", 0.0f));

        yield return new WaitForSeconds(1.75f);

        Transform respawnPoint = GameObject.Find("PointTank_ProteasomeRespawn").transform;
        ship.transform.position = respawnPoint.position;
        ship.transform.rotation = respawnPoint.rotation;
        ship.GetComponent<Rigidbody>().isKinematic = false;
        ship.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Substrate.Instance.ResetSubstrate();

        transform.position = startingPoint;
        transform.rotation = startingRotation;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        shipDebris.Stop(); // stop emission of particles
        _protealizingCutscene = false;
        _protealizing = false;

        wander.enabled = true;

        CameraFade.Instance.StartFade(new Color(0, 0, 0, 0), 1.0f);
        //iTween.CameraFadeTo(iTween.Hash("amount", 0.0f, "time", 1.0f, "delay", 0.0f));

        yield return new WaitForSeconds(1.25f);

        shipControls.enabled = true;


        if (Protealyzed != null) {
            Protealyzed();
        }
        yield return null;
    }
}
