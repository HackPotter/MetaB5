using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Written by Jack Potter, May 2017
 *Scrolls camera down at a given speed for a given time
 *Scrolling credits on the cheap, basically
 *Once the time runs out, it automatically loads the given level*/

public class Credits : MonoBehaviour {

    public GameObject camera;
    public int speed = 1;
    public string level;
    public int runTime;

	
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
        yield return new WaitForSeconds(runTime);
        Application.LoadLevel(level);
    }
}
