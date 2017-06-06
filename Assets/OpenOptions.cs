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

        if(Input.GetKeyDown(KeyCode.Tab)) //Should be ESC, but that messes with the editor. Change before building
        {
             if (GameState.Instance.PauseLevel == PauseLevel.Unpaused) //If the game isn't currently paused...
            {
                _optionsMenu.SetActive(true); //Open the options menu
                GameState.Instance.PauseLevel = PauseLevel.Menu; //Pause the game
                Cursor.lockState = CursorLockMode.None; //Free the cursor
            }
            else //Otherwise...
            {
                _optionsMenu.SetActive(false); //Close the options menu
                _helpScreen.SetActive(false); //Close the help screen if it's open
                GameState.Instance.PauseLevel = PauseLevel.Unpaused; //Unpause the level
                Cursor.lockState = CursorLockMode.Locked; //Lock the cursor back into the game
            }
            
        }
        	
	}

    
}
