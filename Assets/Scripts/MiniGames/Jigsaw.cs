#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Jigsaw : MonoBehaviour
{
    enum Difficulty { CASUAL, CHALLENGE };
    private Difficulty mode;

    private float selectionZpos;
    private int currentID;
    private GameObject currentSelection;
    private Ray ray;
    private RaycastHit hit;

    private GameObject preview;
    private Transform keys;

    private float timer;
    private bool previewing = false;
    private bool playing = false;
    private bool scrambled = false;
    private bool gameOver = false;
    private bool modeSelected = false;
    private bool resetting = false;

    private int piecesFound = 0;
    private int score = 5;

    private int screenWidth;
    private int screenHeight;
    private int rand;

    public GUISkin skin;
    public int numPieces;

    public GameObject wall_1;
    public GameObject wall_2;
    public GameObject wall_3;
    public GameObject wall_4;


    public Texture2D[] textures;
    public Texture2D[] previews;

    public Puzzle[] pieces;

    GameObject TimeLeft;
    Text timeText;

    void Start()
    {
        rand = Random.Range(0, textures.Length);

        selectionZpos = 0.2f + GameObject.Find("PuzzlePiece1").transform.localPosition.z;


        keys = GameObject.Find("keys").transform;
        preview = GameObject.Find("Preview");
        preview.GetComponent<Renderer>().material.SetTexture("_MainTex", previews[rand]);
		
		for(int i = 0; i<36; i++)
		{
			pieces[i].puzzleId = i+1;
			pieces[i].puzzlePiece = GameObject.Find ("PuzzlePiece"+(i+1));			
            pieces[i].puzzlePiece.GetComponent<Renderer>().material.SetTexture("_MainTex", textures[rand]);
            pieces[i].init();
		}
        TimeLeft = GameObject.Find("TimeLeft");
        timeText = TimeLeft.GetComponentInChildren<Text>();
        timeText.text = "";

        /*foreach (Puzzle piece in pieces)
        {
            piece.puzzlePiece.renderer.material.SetTexture("_MainTex", textures[rand]);
            piece.init();
        }*/

        //FIXME_VAR_TYPE dimension= Mathf.Sqrt(numPieces);
        //FIXME_VAR_TYPE grid= GameObject.Find("border").collider.bounds;
        //FIXME_VAR_TYPE gridUL= Vector2(grid.center.x+grid.extents.x , grid.center.y+grid.extents.y);
        /*for(int i = 0; i< dimension; i++){
            FIXME_VAR_TYPE posX= gridUL.x - (grid.size.x/dimension)*i;
            for(int j = 0; j< dimension; j++){
                FIXME_VAR_TYPE posY= gridUL.y - (grid.size.y/dimension)*j;
                pieces[i*6+j].puzzlePiece.renderer.material.SetTexture("_MainTex", textures[rand]);
                pieces[i*6+j].init(Vector2(posX-grid.extents.y/dimension, posY-grid.extents.y/dimension));
            }
        }*/
    }

    void OnGUI()
    {
        GUI.skin = skin;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        GUI.skin.label.fontSize = (int)(Screen.height * 0.025f);
        GUI.skin.button.fontSize = (int)(Screen.height * 0.025f);
        foreach (GUIStyle style in GUI.skin.customStyles)
            style.fontSize = (int)(Screen.height * 0.025f);

        if (!playing)
        {
            // Pieces are invisible
            foreach (Puzzle piece in pieces)
            {
                piece.puzzlePiece.GetComponent<Renderer>().enabled = false; 
            }

            if (!modeSelected)
            {
                previewing = false;
                if (GUI.Button(new Rect(screenWidth * 0.56f, screenHeight * 0.30f, screenWidth * 0.17f, screenHeight * 0.05f), "Casual"))
                {
                    mode = Difficulty.CASUAL;
                    modeSelected = true;

                }
                if (GUI.Button(new Rect(screenWidth * 0.56f, screenHeight * 0.40f, screenWidth * 0.17f, screenHeight * 0.05f), "Challenge"))
                {
                    mode = Difficulty.CHALLENGE;
                    modeSelected = true;
                }
            }
            else
            {
                scramble();

                //if (resetting)
                {
                  //  reset();
                }
               // else
                {
                    // Play preview for 3 seconds
                    if (!previewing)
                    {
                        timer = Time.time;
                        preview.GetComponent<Renderer>().enabled = true;
                        previewing = true;
                    }
                    else if (Time.time - timer > 0f)
                    {
                        preview.GetComponent<Renderer>().enabled = true;
                        timer = Time.time;
                        playing = true;
                    }
                }
            }
        }

        else
        {
            float timeLeft = Mathf.Round(numPieces * 10 - (Time.time - timer));	// Time to complete challenge mode
            int timeinMins = (int)(timeLeft / 60);

            // Pieces are visible
            foreach (Puzzle piece in pieces)
            {
                piece.puzzlePiece.GetComponent<Renderer>().enabled = true;
            }

            GameObject modeObject = GameObject.FindGameObjectWithTag("JigsawMode");
            Text modeText = modeObject.GetComponentInChildren<Text>();
            modeText.text = "Mode:";
            if (mode == Difficulty.CASUAL){
                modeText.text = " Mode: Casual";
            }
            else{
                modeText.text = " Mode: Challenge";
            }
        
            GameObject ScorePoints = GameObject.FindGameObjectWithTag("JigsawScorePoints");
            Text scoreP = ScorePoints.GetComponentInChildren<Text>();
            if (!gameOver){
                //TO Display the Name of Puzze loaded
                //GUI.Label(new Rect(screenWidth * 0.07f, screenHeight * 0.68f, screenWidth * 0.17f, screenHeight * 0.05f), textures[rand].name.Substring(10).ToUpper());

                if (mode == Difficulty.CHALLENGE){
                    timeText.text = "Time Left:" + timeinMins + " : " + (timeLeft % 60).ToString("00");

                    if (piecesFound == numPieces){
                        score += 15;
                        scoreP.text = score.ToString();
                        gameOver = true;
                        GameContext.Instance.Player.Points += score;
                    }
                    if (timeLeft <= 0){
                        gameOver = true;
                    }
                }
                else{
                    if (piecesFound == numPieces){
                        score += 10;
                        scoreP.text = score.ToString();
                        gameOver = true;
                        GameContext.Instance.Player.Points += score;
                    }
                }
            }
        }

        if (gameOver){
            playing = false;

            if (piecesFound < 36){
                GUI.Label(new Rect(screenWidth * 0.32f, screenHeight * 0.70f, screenWidth * 0.65f, screenHeight * 0.1f), "Sorry! Better luck next time! \n\n You can play again or hit the Close button above.");
                if (!resetting){
                    timer = Time.time;
                    resetting = true;
                }
            }
            else{
                preview.GetComponent<Renderer>().enabled = true;

                GUI.Label(new Rect(screenWidth * 0.57f, screenHeight * 0.70f, screenWidth * 0.15f, screenHeight * 0.05f), "You won!");

                /*
                if (sceneToLoad != null)
                {
                    GUI.Label(new Rect(screenWidth * 0.32f, screenHeight * 0.75f, screenWidth * 0.65f, screenHeight * 0.1f), "Click on the transport system on the image to go to the next scene!");

                    foreach (Transform key in keys)
                        key.gameObject.layer = 0;
                    GUI.Box(new Rect(Input.mousePosition.x - screenHeight * 0.05f, screenHeight * 0.95f - Input.mousePosition.y, screenHeight * 0.1f, screenHeight * 0.1f), "", "Selector");
                    if (currentSelection && currentSelection.name == "key" && sceneToLoad != null)
                    {
                        LoaderUI.nextLevel = sceneToLoad;
                        Application.LoadLevel("Loader");
                    }
                }*/
            }
        }
    }
   
    void Update(){

        checkInput();
        checkBorders();		// Make sure player doesn't exceed borders
        snapToGrid();		//Snap pieces to grid system
    }

    void scramble()
    {
        if (!scrambled)
        {
            foreach (Puzzle piece in pieces)
            {
                if (mode == Difficulty.CHALLENGE)
                    piece.puzzlePiece.transform.Rotate(0, 0, 90 * Random.Range(0, 4));

                Vector3 position = piece.puzzlePiece.transform.position;
                position.x = Random.Range(-12, 18);
                position.y = Random.Range(-4, 1);
                piece.puzzlePiece.transform.position = position;
            }
            scrambled = true;
        }
        
    }

    public void reset()
    {
        piecesFound = 0;
        scrambled = false;
        gameOver = false;
        modeSelected = false;
        resetting = false;
        previewing = false;
        playing = false;
        Start();
        scramble();
    }

    void checkInput()
    {
        if (Input.GetButtonDown("Primary"))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Jigsaw")
                {
                    currentSelection = hit.collider.gameObject;
                    currentID = int.Parse(currentSelection.name.Substring(11));
                }
                else if (hit.collider.name == "key")
                {
                    currentSelection = hit.collider.gameObject;
                }
                else
                {
                    currentSelection = null;
                }
            }
        }

       if (currentSelection && currentSelection.tag == "Jigsaw")
        {
            // Rotate
            if (mode == Difficulty.CHALLENGE)
            {
                if (Input.GetKeyDown("e"))
                    currentSelection.transform.Rotate(0, 0, -90);
                if (Input.GetKeyDown("r"))
                    currentSelection.transform.Rotate(0, 0, 90);
            }

            // Translate
            if (Input.GetButton("Primary"))
            {
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    currentSelection.transform.position = new Vector3(curPosition.x, curPosition.y, currentSelection.transform.position.z);
                }
            }
        }
    }

    void checkBorders()
    {
        if (currentSelection && currentSelection.transform.position.x >= wall_1.transform.position.x)
        {
            currentSelection.transform.position = currentSelection.transform.position.WithX(wall_1.transform.position.x);
        }
        if (currentSelection && currentSelection.transform.position.y >= wall_2.transform.position.y)
        {
            currentSelection.transform.position = currentSelection.transform.position.WithY(wall_2.transform.position.y);
        }
        if (currentSelection && currentSelection.transform.position.x <= wall_3.transform.position.x)
        {
            currentSelection.transform.position = currentSelection.transform.position.WithX(wall_3.transform.position.x);
        }
        if (currentSelection && currentSelection.transform.position.y <= wall_4.transform.position.y)
        {
            currentSelection.transform.position = currentSelection.transform.position.WithY(wall_4.transform.position.y);
        }
    }
    void snapToGrid()
    {
        if (currentSelection)
        {
            foreach (Puzzle piece in pieces)
            {
                if (piece.posMatches(currentSelection))
                {
                    currentSelection.transform.localPosition = currentSelection.transform.localPosition.WithX(piece.getInitPos().x);
                    currentSelection.transform.localPosition = currentSelection.transform.localPosition.WithY(piece.getInitPos().y);

                    if (Input.GetButtonUp("Primary"))
                    {
                        currentSelection.transform.localPosition = currentSelection.transform.localPosition.WithZ(piece.getInitPos().z);
                        if (currentID == piece.puzzleId && piece.rotMatches(currentSelection))
                        {
                            currentSelection.transform.localEulerAngles = currentSelection.transform.localEulerAngles.WithZ(piece.getInitZRotAngle());
                            currentSelection.layer = 2;
                            piecesFound++;
                        }
                        currentSelection = null;
                    }
                    break;
                }
            }
        }
    }
	
	[System.Serializable]
    public class Puzzle
    {
        private Vector3 initPos;
        private float initZRotAngle;
        public int puzzleId;
        public GameObject puzzlePiece;

        public void init()
        {
            this.initPos = this.puzzlePiece.transform.localPosition;
            this.initZRotAngle = this.puzzlePiece.transform.localEulerAngles.z;
        }

        public Vector3 getInitPos()
        {
            return this.initPos;
        }

        public float getInitZRotAngle()
        {
            return this.initZRotAngle;
        }

        public bool posMatches(GameObject other)
        {
            if (Vector3.Distance(this.initPos, other.transform.localPosition) <= 1.5f)
                return true;
            return false;
        }

        public bool rotMatches(GameObject other)
        {
            if (Mathf.Abs(this.initZRotAngle - other.transform.localEulerAngles.z) <= 5)
                return true;
            return false;
        }
        
    }
}