using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour {

    public GameObject camera;
    public int speed = 1;
    public string level;

	
	// Update is called once per frame
	void Update () {

        camera.transform.Translate(Vector3.down * Time.deltaTime * speed);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(level);
        }

        StartCoroutine(waitFor());
	}

    IEnumerator waitFor()
    {
        yield return new WaitForSeconds(107);
        Application.LoadLevel(level);
    }
}
