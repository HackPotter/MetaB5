using System.Collections;
using UnityEngine;

public class CreditFading : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private GameObject[] _gameObjects;

    [SerializeField]
    private int[] _timings;
#pragma warning restore 0067, 0649

    void Start ()
    {
        StartCoroutine(Fade());
    }

    void Update ()
    {

    }

    IEnumerator Fade()
    {
        for(int i = 0; i < _gameObjects.Length; i++)
        {
            _gameObjects[i].SetActive(true);
            yield return new WaitForSecondsRealtime(_timings[i]);
            _gameObjects[i].SetActive(false); 
        }
    }
}


