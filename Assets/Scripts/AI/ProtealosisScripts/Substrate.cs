using System;
using UnityEngine;

[AddComponentMenu("Metablast/Protealosis/Substrate")]
public class Substrate : MonoBehaviour
{
    public static event Action ShipFullyUbiquintated;
    private static Substrate _instance;

    [SerializeField]
    private float _ubiquitinDecayRate = 2.0f;
    private float _lastDecayTime = 0.0f;
    private int _hitCount = 0;

    private GameObject[] _substrates;

    public static Substrate Instance
    {
        get
        {
            return _instance;
        }
    }

    public int UbiquitinCount
    {
        get { return _hitCount; }
    }

    public bool CanBeUbiquinated
    {
        get { return _hitCount != 8; }
    }

    public bool CanBeProtealized
    {
        get { return _hitCount == 8; }
    }

    public void AttachUbiquitin()
    {
        if (_hitCount <= 7)
        {
            _substrates[_hitCount].SetActive(true);
            _hitCount++;
            _lastDecayTime = Time.time;
            if (_hitCount == 8)
            {
                if (ShipFullyUbiquintated != null)
                {
                    ShipFullyUbiquintated();
                }
            }
        }   
    }

    public void ResetSubstrate()
    {
        _hitCount = 0;
        foreach (var substrate in _substrates)
        {
            substrate.SetActive(false);
        }
    }

    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Error: Multiple instances of Substrate. Substrate is a singleton.");
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        _substrates = new GameObject[8];
        
        _substrates[0] = transform.FindChildInHierarchy("Substrate0").gameObject;
        _substrates[1] = transform.FindChildInHierarchy("Substrate1").gameObject;
        _substrates[2] = transform.FindChildInHierarchy("Substrate2").gameObject;
        _substrates[3] = transform.FindChildInHierarchy("Substrate3").gameObject;
        _substrates[4] = transform.FindChildInHierarchy("Substrate4").gameObject;
        _substrates[5] = transform.FindChildInHierarchy("Substrate5").gameObject;
        _substrates[6] = transform.FindChildInHierarchy("Substrate6").gameObject;
        _substrates[7] = transform.FindChildInHierarchy("Substrate7").gameObject;

        ResetSubstrate();
    }

    void Update()
    {
        if (_hitCount == 0)
        {
            _lastDecayTime = Time.time;
        }
        if (_hitCount > 0 && Time.time - _lastDecayTime > _ubiquitinDecayRate)
        {
            _substrates[_hitCount - 1].SetActive(false);
            _hitCount--;
            _lastDecayTime = Time.time;
        }
    }
}

