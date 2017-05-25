using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
