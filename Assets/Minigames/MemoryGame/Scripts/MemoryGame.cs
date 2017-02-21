// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MemoryGame : MonoBehaviour
{

    public GUISkin skin;
	public Texture2D[] textures = new Texture2D[4];
   	
	private Ray ray;
    private RaycastHit hit;
    private int i;
	private int version = 0;
	private int totalTiles;
    private GameObject[] tiles;
    private GameObject tile1;
    private GameObject tile2;
    private int tile1ID;
    private int tile2ID;
    private int[] matches = new int[] { 2, 1, 4, 3, 6, 5, 8, 7, 10, 9, 12, 11, 14, 13, 16, 15, 18, 17, 20, 19, 22, 21, 24, 23 };
    private GameObject g;
    private Color tilecolor;
    private Vector3 safeposition;
    private bool[] destroyarray;
    private bool[] resetarray;
    private Vector3[] positions;
    private int[] myRandomNumbers;
    private List<int> mynumbers = new List<int>();
    private int clickCount = 0;
    private int localscore = 0;
    private int mismatch = 0;
    private bool finish = true;
    private bool congrats = false;
    private bool sorry = false;
    private bool hitter = false;
    private bool skipWait = false;
    private int starttime;
    private int myTime;


    private bool modeSelected = false;
    private bool challenge = false;
    private bool insane = false;
    private bool cheater = false; //if true shows position of matching tile, used for debug
    private int howmuchtime = 80; //must match timeleft
    private int timeleft = 80; //^
    private int chal_attempt = 50; //actually double the attempts allowed in game
    private int insane_attempt = 40; //double the attempts



    void Start()
    {
        safeposition = GameObject.Find("Main_Camera").transform.localPosition;
		versionControl ();
    }


    void Update()
    {
        if (Input.GetButtonDown("Primary"))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hitter)
            {
                if (hit.collider.tag == "Tile")
                {
                    clickCount++;

                    if (clickCount == 1)
                    {
                        tile1 = hit.collider.gameObject;
                        tile1ID = int.Parse(tile1.name.Substring(5));
                        print(tile1ID);
                        tile1.GetComponent<Animation>().Play("TileAnimation1");
                        if (cheater)
                            GameObject.Find("Tile_" + matches[tile1ID - 1]).GetComponent<Renderer>().material.color = Color.green;
                    }
                    else if (clickCount == 2)
                    {
                        clickCount = 0;
                        tile2 = hit.collider.gameObject;
                        tile2ID = int.Parse(tile2.name.Substring(5));
                        tile2.GetComponent<Animation>().Play("TileAnimation1");
                        print(tile2ID);
                        if (cheater)
                        {
                            GameObject.Find("Tile_" + matches[tile1ID - 1]).GetComponent<Renderer>().material.color = tilecolor;
                        }
                        if (matches[tile2ID - 1] == tile1ID)
                        {
                            destroyarray[tile1ID - 1] = true;
                            destroyarray[tile2ID - 1] = true;
                            for (i = 0; i < totalTiles; i++)
                            {
                                if (destroyarray[i])
                                    StartCoroutine(destroytile(i));
                            }
                        }
                        else
                        {
                            resetarray[tile1ID - 1] = true;
                            resetarray[tile2ID - 1] = true;
                            for (i = 0; i < totalTiles; i++)
                            {
                                if (resetarray[i])
                                    StartCoroutine(resettile(i));
                            }
                            if (tile1ID == tile2ID)
                                mismatch += 1;
                        }
                    }
                }
            }
        }

        //end game checks
        if (localscore == totalTiles && finish == true)
        {
            timeleft = 0;
            StartCoroutine(completion());
        }

        if (challenge)
        {
            if (mismatch == chal_attempt && localscore < totalTiles && finish == true)
                StartCoroutine(failure());
        }

        if (insane)
        {
            if ((mismatch == insane_attempt || timeleft == 0) && localscore < totalTiles && finish == true)
            {
                timeleft = 0;
                StartCoroutine(failure());
            }

            myTime = (int)(Time.time - starttime);

            if (timeleft > 0)
            {
                timeleft = howmuchtime - myTime;
            }
        }
    }


    //reset and destroy functions
    IEnumerator resettile(int resetID)
    {
        resetarray[resetID] = false;
       	yield return new WaitForSeconds(1.3f);
        if (!skipWait)
        { print ("animation2");
            tiles[resetID].GetComponent<Animation>().Play("TileAnimation2");
        }
        if (challenge && mismatch < chal_attempt) { mismatch += 1; }
        if (insane && mismatch < insane_attempt) { mismatch += 1; }
    }

    IEnumerator destroytile(int destroyID)
    {
        destroyarray[destroyID] = false;
        if (!skipWait)
        {
            yield return new WaitForSeconds(1.3f);
            localscore += 1;
        }
        GameObject.Destroy(tiles[destroyID]);
    }

    void resetalltiles()
    {
        for (i = 0; i < totalTiles; i++)
        {
            GameObject.Find("Tile_" + (i + 1) + "(Clone)").transform.localPosition = positions[i];
            GameObject.Find("Tile_" + (i + 1) + "(Clone)").transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


    //end game functions
    IEnumerator completion()
    {
        hitter = false;
        finish = false;
        yield return new WaitForSeconds(1.0f);
        congrats = true;
        yield return new WaitForSeconds(3.5f);
        congrats = false;
        resetalltiles();
    }

    IEnumerator failure()
    {
        hitter = false;
        finish = false;
        skipWait = true;
        yield return new WaitForSeconds(1.0f);
        for (i = 0; i < totalTiles; i++)
        {
            StartCoroutine(destroytile(i));
        }
        yield return new WaitForSeconds(0.5f);
        sorry = true;
        yield return new WaitForSeconds(3.5f);
        sorry = false;
        resetalltiles();
    }


    void versionControl()
	{
		if(version == 0)
		{
			version = 1;// Random.Range (1,totalVersions+1);
		
			if(version == 1)
			{
				totalTiles = 24;
				tiles = new GameObject[24];
		    	destroyarray = new bool[24];
		    	resetarray = new bool[24];
		    	positions = new Vector3[24];
		    	myRandomNumbers = new int[24];
			}
			else{
				totalTiles = 16;
				tiles = new GameObject[16];
		    	destroyarray = new bool[16];
		    	resetarray = new bool[16];
		    	positions = new Vector3[16];
		    	myRandomNumbers = new int[16];
				
				for (i = totalTiles; i < 24; i++)
		        {
		            GameObject.Find("Card_" + (i + 1)).SetActive(false);
				}
			}
			
			for (i = 0; i < totalTiles; i++)
	        {
	            g = GameObject.Find("Tile_" + (i + 1));
				g.GetComponent<Renderer>().material.mainTexture = textures[version-1];
				g.GetComponent<Renderer>().enabled = true;
	            destroyarray[i] = false;
	            resetarray[i] = false;
	            tiles[i] = g;
	            GameObject.Instantiate(tiles[i]);
	            GameObject.Find("Tile_" + (i + 1) + "(Clone)").transform.localPosition = safeposition;
	            positions[i] = GameObject.Find("Card_" + (i + 1)).transform.localPosition;
	            mynumbers.Add(i);
			}
			
			tilecolor = tiles[0].GetComponent<Renderer>().material.color;

			for (i = 0; i < totalTiles; i++)
	        {
	            int rand = Random.Range(0, mynumbers.Count);
	            myRandomNumbers[i] = mynumbers[rand];
	            mynumbers.RemoveAt(rand);
	
	            if (i == totalTiles - 1)
	            {
	                for (i = 0; i < totalTiles; i++)
	                {
	                    GameObject.Find("Card_" + (i + 1)).transform.localPosition = positions[myRandomNumbers[i]];
	                }
	            }
	        }	
		}
	}
	
	void OnGUI()
    {
        GUI.skin = skin;

        GUI.skin.label.fontSize = (int)(Screen.height * 0.025f);
        GUI.skin.button.fontSize = (int)(Screen.height * 0.025f);
        foreach (GUIStyle style in GUI.skin.customStyles)
            style.fontSize = (int)(Screen.height * 0.025f);

        if (GUI.Button(new Rect(139 * Screen.width / 160, 73 * Screen.height / 80, Screen.width / 13, Screen.height / 13), "", "Back"))
        {
			Application.LoadLevel("Laboratory");
        }

        if (!modeSelected)
        {
			//GUI.Box(new Rect(89 * Screen.width / 160, Screen.height / 7, Screen.width / 6, Screen.height / 20), "WELCOME TO THE MEMORY GAME", "Score");
            GUI.Box(new Rect(Screen.width / 12, Screen.height / 12, Screen.width / 16, Screen.height / 20), "Select Difficulty:", "Score");

            if (GUI.Button(new Rect(Screen.width / 70, 4 * Screen.height / 24, Screen.width / 5, Screen.height / 13), "Casual"))
            {
                modeSelected = true;
                hitter = true;
            }

            if (GUI.Button(new Rect(Screen.width / 70, 6 * Screen.height / 24, Screen.width / 5, Screen.height / 13), "Challenge"))
            {
                challenge = true;
                modeSelected = true;
                hitter = true;
            }

            if (GUI.Button(new Rect(Screen.width / 70, 8 * Screen.height / 24, Screen.width / 5, Screen.height / 13), "Insane"))
            {
                insane = true;
                modeSelected = true;
                hitter = true;
                starttime = (int)Time.time;
            }
        }


        else
        {
			GUILayout.BeginArea(new Rect(25,25, 500, Screen.height));
			
			GUILayout.Label("Matches found: " + localscore / 2 + "/"+ totalTiles/2, "Score");
			
			GUILayout.Space(25);
			
			if (challenge)
			{
				GUILayout.Label("Mismatches left: " + (chal_attempt - mismatch) / 2 + "/" + chal_attempt / 2, "Score");
			}
			else if (insane)
			{
				GUILayout.Label("Mismatches left: " + (insane_attempt - mismatch) / 2 + "/" + insane_attempt / 2, "Score");
			}
			else
			{
				GUILayout.Label("Mismatches left: unlimited", "Score");
			}
			
			GUILayout.Space(25);
			
			if (insane)
            {
                GUILayout.Label("Timer: " + timeleft, "Timer");
                if (timeleft < 15 && timeleft > 0 && timeleft % 2 == 0)
                {
                    GUILayout.Box("WARNING: TIME LOW", "Score");
                }
            }
			
			GUILayout.Space(25);
			if (congrats)
			{
                GUILayout.Label("Congratulations, you found them all!", "Score");
			}

            if (sorry)
			{
                GUILayout.Label("I'm sorry, you have failed", "Score");
			}
			
			GUILayout.EndArea();
			
			/*
            GUI.Label(new Rect(Screen.width / 180, 4 * Screen.height / 24, Screen.width / 6, Screen.height / 16), "Matches found: " + localscore / 2 + "/"+ totalTiles/2, "Score");

            if (challenge)
            {
                GUI.Label(new Rect(Screen.width / 180, 6 * Screen.height / 24, Screen.width / 6, Screen.height / 16), "Mismatches left: " + (chal_attempt - mismatch) / 2 + "/" + chal_attempt / 2, "Score");
            }
            else if (insane)
            {
                GUI.Label(new Rect(Screen.width / 180, 6 * Screen.height / 24, Screen.width / 6, Screen.height / 16), "Mismatches left: " + (insane_attempt - mismatch) / 2 + "/" + insane_attempt / 2, "Score");
            }
            else
            {
                GUI.Label(new Rect(Screen.width / 180, 6 * Screen.height / 24, Screen.width / 6, Screen.height / 16), "Mismatches left: unlimited", "Score");
            }

            if (insane)
            {
                GUI.Label(new Rect(Screen.width / 180, 6 * Screen.height / 24, Screen.width / 6, Screen.height / 16), "Timer: " + timeleft, "Timer");
                if (timeleft < 15 && timeleft > 0 && timeleft % 2 == 0)
                {
                    GUI.Box(new Rect(89 * Screen.width / 160, 5 * Screen.height / 28, Screen.width / 6, Screen.height / 20), "WARNING: TIME LOW", "Score");
                }
            }


            if (congrats)
                GUI.Label(new Rect(43 * Screen.width / 80, 3 * Screen.height / 7, Screen.width / 5, Screen.height / 20), "Congratulations, you found them all!", "Score");

            if (sorry)
                GUI.Label(new Rect(43 * Screen.width / 80, 3 * Screen.height / 7, Screen.width / 5, Screen.height / 20), "I'm sorry, you have failed", "Score");*/
        }
    }
}