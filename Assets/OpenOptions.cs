using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOptions : MonoBehaviour {

    [SerializeField]
    private GameObject _optionsMenu;
    [SerializeField]
    private GameObject _helpScreen;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameState.Instance.PauseLevel == PauseLevel.Unpaused)
            {
                _optionsMenu.SetActive(true);
                GameState.Instance.PauseLevel = PauseLevel.Menu;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                _optionsMenu.SetActive(false);
                _helpScreen.SetActive(false);
                GameState.Instance.PauseLevel = PauseLevel.Unpaused;
                Cursor.lockState = CursorLockMode.Locked;
            }
            
        }
        	
	}

    
}
