using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOptions : MonoBehaviour {

    [SerializeField]
    private GameObject _optionsMenu;
    private bool toggle;

	// Use this for initialization
	void Start () {
        toggle = true;
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (toggle)
            {
                _optionsMenu.SetActive(true);
                GameState.Instance.PauseLevel = PauseLevel.Menu;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                _optionsMenu.SetActive(false);
                GameState.Instance.PauseLevel = PauseLevel.Unpaused;
                Cursor.lockState = CursorLockMode.Locked;
            }
            
            toggle = !toggle;
        }
        	
	}

    
}
