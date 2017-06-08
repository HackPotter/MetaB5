using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    public GameObject _camera;
    public int speed = 1;
    public string level;

	
	// Update is called once per frame
	void Update () {

        _camera.transform.Translate(Vector3.down * Time.deltaTime * speed);

        if(Input.GetKeyDown(KeyCode.Escape))
        {

            SceneManager.LoadScene(level);
        }

        StartCoroutine(waitFor());
	}

    IEnumerator waitFor()
    {
        yield return new WaitForSeconds(107);
        SceneManager.LoadScene(level);
    }
}
