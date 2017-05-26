using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Metablast/Protealosis/Ubiquitin Ligase Agent")]
public class UbiquitinLigaseAgent : Agent
{
    public static event Action ShipUbiquinated;

    //public Tether tether;
    public Wander wander;
    public Pursue pursue;

    private Substrate _substrate;

    private static float _lastLigaseHitTime = 0;
    private static int hitRate = 0;
    private static List<UbiquitinLigaseAgent> _pursuingAgent = new List<UbiquitinLigaseAgent>();

    private GameObject _ubiquitinParticle;


    private float _lastLoadTime = 0;

    public int PursueCount = 0;
    public bool AnyLigasePursuing = false;
    public GameObject PursuingUbiquitin = null;
    public int _maxPursuers = 3;
    public float _reloadTime = 7.5f;
    public float _pursuitAcceleration = 6f;
    public float _pursuitSpeed = 5f;
    public float _wanderAcceleration = 6f;
    public float _wanderSpeed = 5f;
    public bool _isPursuing = false;
    private bool _loaded = false;
	
	private PauseLevel _lastPauseLevel;
	private Vector3 _unpausedVelocity;


    void Awake()
    {
        /**Initialization**/

        /******Fix: random rotation behavior***/


        //get the parent object which should be the game object with the sphere collider component
        Transform parent = transform.parent;

        //get the UbiquitinTriggerScript component attached to the parent
        //ubiquitinTriggerScript = parent.GetComponent<UbiquitinTriggerScript>();
        _ubiquitinParticle = transform.FindChild("UbiquitinMolecule").gameObject;

        //set the tether parameters
        //tether.SetTetherPosition(parent.position);

        // Error here. Parent doesn't have a sphere collider.
        //tether.SetTetherRadius(parent.GetComponent<SphereCollider>().radius);

        //default behavior for Ubiquiting agent is wander

        //LoadUbiquitinParticle();
    }
	
	void OnDestroy()
	{
		_pursuingAgent.Remove(this);
	}
	
    void Start()
    {
        _substrate = Substrate.Instance;
    }

    private void LoadUbiquitinParticle()
    {
        _loaded = true;
        _ubiquitinParticle.GetComponent<Renderer>().enabled = true;
    }

    private void UnloadUbiquitinParticle()
    {
        _loaded = false;
        _ubiquitinParticle.GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        PursueCount = _pursuingAgent.Count;
		if (GameState.Instance.PauseLevel != PauseLevel.Unpaused && _lastPauseLevel == PauseLevel.Unpaused)
		{
			_unpausedVelocity = GetComponent<Rigidbody>().velocity;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			return;
		}
		else if (GameState.Instance.PauseLevel == PauseLevel.Unpaused && _lastPauseLevel != PauseLevel.Unpaused)
		{
			GetComponent<Rigidbody>().velocity = _unpausedVelocity;
		}
		_lastPauseLevel = GameState.Instance.PauseLevel;
        AnyLigasePursuing = _pursuingAgent.Count != 0;
        if (AnyLigasePursuing)
        {
            //PursuingUbiquitin = _pursuingAgent.gameObject;
        }
        if (!_loaded && Time.time - _lastLoadTime > _reloadTime)
        {
            LoadUbiquitinParticle();
            _lastLoadTime = Time.time;
        }
        if (_pursuingAgent.Contains(this))
        {
            pursue.Weight = 1;
            wander.Weight = 0;
			
			Vector3 velocity = pursue.GetComponent<Rigidbody>().velocity.normalized;
			Vector3 directionToTarget = (pursue.GetTarget().transform.position - transform.position).normalized;
			
			if (Vector3.Dot(velocity, directionToTarget) < 0 && pursue.GetComponent<Rigidbody>().velocity.magnitude > _pursuitSpeed * 0.5f)
			{
                _pursuingAgent.Remove(this);
			}
            /*
            if (!ubiquitinTriggerScript.IsShipDetected())
            {
                maxSpeed = _wanderSpeed;
                maxAcceleration = _wanderAcceleration;
                _isPursuing = false;
                _pursuingAgent = null;
            }*/
        }
        else
        {
            pursue.Weight = 0;
            wander.Weight = 1;
        }

        ResolveCurrentBehavior();
        if (!_substrate.CanBeUbiquinated)
        {
            MaxSpeed = _wanderSpeed;
            MaxAcceleration = _wanderAcceleration;
            _isPursuing = false;
            _pursuingAgent.Clear();
        }
    }

    void ResolveCurrentBehavior()
    {
        if (!_substrate.CanBeUbiquinated)
        {
            return;
        }
        if (_pursuingAgent.Count < _maxPursuers &&
			_loaded &&
			(Time.time - _lastLigaseHitTime > hitRate) &&
			UnityEngine.Random.value < 0.3f)
        {
			float distanceToTarget = Vector3.Distance(transform.position, pursue.GetTarget().transform.position);
			if (distanceToTarget > 200 && distanceToTarget < 800)
			{
            	MaxSpeed = _pursuitSpeed;
            	MaxAcceleration = _pursuitAcceleration;
            	_pursuingAgent.Add(this);
            	_isPursuing = true;
			}
        }
        else if (!_pursuingAgent.Contains(this))
        {
            MaxSpeed = _wanderSpeed;
            MaxAcceleration = _wanderAcceleration;
            _isPursuing = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_loaded && ((Time.time - _lastLigaseHitTime > hitRate) || !_isPursuing) && collision.gameObject.name.Equals("Tank"))
        {
            UnloadUbiquitinParticle();
            _lastLoadTime = Time.time;
            _lastLigaseHitTime = Time.time;
            if (_substrate.CanBeUbiquinated)
            {
                if (ShipUbiquinated != null)
                {
                    ShipUbiquinated();
                }
                _substrate.AttachUbiquitin();
            }
            MaxSpeed = _wanderSpeed;
            MaxAcceleration = _wanderAcceleration;
            _isPursuing = false;
            _pursuingAgent.Remove(this);
        }
    }
}