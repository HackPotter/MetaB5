using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/* 
 * Written by Jack Potter, May 2017
 * Controls credit fading for top billing credits.
 * Each object added in unity will remain on screen for the time specified in the "timings" array with the same index
 */


public class CreditFading : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private GameObject[] _gameObjects;

#pragma warning disable 0067, 0649
    [SerializeField]
    private GameObject _scrollCredits;

    [SerializeField]
    private int[] _timings;
#pragma warning restore 0067, 0649

    void Start()
    {
        StartCoroutine(Fade());
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator Fade()
    {
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            _gameObjects[i].SetActive(true);
            yield return new WaitForSecondsRealtime(_timings[i]);
            _gameObjects[i].SetActive(false);
        }
        _scrollCredits.SetActive(true);
    }
}
