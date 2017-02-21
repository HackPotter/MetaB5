// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class DotMatrix : MonoBehaviour
{
    private int level;
    private int step;
    private int score = 0;
    private int matches = 0;	// Number of correct toggles that are on
    private int clicks = 0;	// Total number of toggles that are on
    private int finalQuestion;
    private int i;
    private int j;

    private float screenW;
    private float screenH;

    private bool transitionVisible = false;
    private bool gameStarted = false;
    private bool playing = false;
    private bool drawing = false;
    private bool highlighting = false;
    private bool[] toggle = new bool[81];
    private bool[] answers = new bool[81];


    private string[] optionsH;
    private string[] optionsV;
    private string intro;
    private string message;

    private Texture2D image;

    public Texture2D goodEx;
    public Texture2D badEx;
    public Texture2D question1;
    public Texture2D question2;
    public Texture2D background;
    public Texture2D blackOverlay;

    public GUISkin skin;

    void Start()
    {
        for (i = 0; i < 81; i++)
        {
            toggle[i] = false;
            answers[i] = false;
        }
    }

    void OnGUI()
    {
        screenW = Screen.width;
        screenH = Screen.height;

        if (this.GetComponent<Camera>().enabled)
        {
            GUI.skin = skin;

            GUI.skin.label.fontSize = (int)(screenH * 0.017f);
            GUI.skin.button.fontSize = (int)(screenH * 0.015f);
            foreach (GUIStyle style in GUI.skin.customStyles)
                style.fontSize = (int)(screenH * 0.018f);

            GUI.DrawTexture(new Rect(0, 0, screenW, screenH * 0.95f), background);

            GUI.Label(new Rect(screenW * 0.07f, screenH * 0.25f, screenW * 0.15f, screenH * 0.05f), "Score: " + score);


            if (!gameStarted)
            {
                step = 1;
                level = 1;
                score = 0;
                playing = false;
                drawing = false;
                GUI.Label(new Rect(screenW * 0.37f, screenH * 0.3f, screenW * 0.55f, screenH * 0.3f), "A 'Dot-matrix' is one way scientists determine how similar two sequences are. \n \n \n Try to click on the matches between the corresponding row and column of the nucleotide sequences... ");
                if (GUI.Button(new Rect(screenW * 0.6f, screenH * 0.5f, screenW * 0.1f, screenH * 0.05f), "Start!"))
                {
                    finalQuestion = Random.Range(0, 3);
                    gameStarted = true;
                }
            }
            else
            {
                //				GUI.Label(new Rect(screenW*0.07f, screenH*0.1f, screenW*0.15f, screenH*0.05f), "Drawing: " + drawing+"\nPlaying: "+ playing);		
                //				GUI.Label(new Rect(screenW*0.07f, screenH*0.15f, screenW*0.15f, screenH*0.05f), "Level: " + level);
                //				GUI.Label(new Rect(screenW*0.07f, screenH*0.2f, screenW*0.15f, screenH*0.05f), "Step: " + step);
                //				GUI.Label(new Rect(screenW*0.07f, screenH*0.3f, screenW*0.15f, screenH*0.05f), "Clicks: " + clicks);
                //				GUI.Label(new Rect(screenW*0.07f, screenH*0.35f, screenW*0.15f, screenH*0.05f), "Matches: " + matches);

                if (drawing)
                {
                    if (level == 3)
                    {
                        drawQuestion();
                    }
                    else
                    {
                        drawLevel();
                    }
                }

                if (!playing)
                {
                    if (step == 1)
                    {
                        GUI.Label(new Rect(screenW * 0.4f, screenH * 0.4f, screenW * 0.5f, screenH * 0.1f), intro);
                        if (GUI.Button(new Rect(screenW * 0.6f, screenH * 0.5f, screenW * 0.1f, screenH * 0.05f), "Continue!"))
                        {
                            playing = true;
                            drawing = true;
                        }
                    }
                    else if (step == 2)
                    {
                        if (transitionVisible)
                        {
                            GUI.DrawTexture(new Rect(screenW * 0.34f, screenH * 0.12f, screenW * 0.63f, screenH * 0.70f), blackOverlay);
                            GUI.Label(new Rect(screenW * 0.35f, screenH * 0.25f, screenW * 0.59f, screenH * 0.45f), new GUIContent(message, image), "Content");
                        }
                        StartCoroutine(nextLevel());
                    }
                    else if (step == 3)
                    {
                        if (transitionVisible)
                        {
                            GUI.Label(new Rect(screenW * 0.4f, screenH * 0.4f, screenW * 0.5f, screenH * 0.1f), message);
                        }

                        StartCoroutine(endGame());
                    }
                }
            }

            if (GUI.Button(new Rect(screenW * 0.875f, screenH * 0.935f, screenW * 0.07f, screenH * 0.05f), "", "Back"))
            {
                exit();
            }
        }
    }

    void Update()
    {
        if (!drawing)
        {
            matches = 0;
            clicks = 0;
            highlighting = false;
            for (i = 0; i < 81; i++)
            {
                toggle[i] = false;
                answers[i] = false;
            }
        }

        if (level == 1)
        {
            optionsH = new string[] { "A", "G", "G", "C", "T", "T", "C", "A", "G" };
            optionsV = new string[] { "A", "G", "G", "C", "T", "T", "C", "T", "C" };
            intro = "First let's take a look at good alignments.";
            message = "\n\nThese sequences are more closely aligned than would be expected by chance.\n \n SCORE: 5 points";
            image = goodEx;
        }
        else if (level == 2)
        {
            optionsH = new string[] { "A", "G", "G", "C", "T", "T", "C", "A", "C" };
            optionsV = new string[] { "G", "A", "C", "C", "T", "A", "T", "A", "G" };
            intro = "Now let's take a look at bad alignments.";
            message = "\n\nThese sequences are less closely aligned than would be expected by chance.\n \n SCORE: -3 points";
            image = badEx;
        }
        else if (level == 3)
        {
            intro = "Now let's review what we've learned! Click on this button to continue.";
            message = "Now, you have a basic understanding of Dot Matrices! \n\nHit the BACK button to exit.";
        }
    }

    private void drawLevel()
    {
        //Draw Horizontal Labels	
        GUILayout.BeginArea(new Rect(screenW * 0.41f, screenH * 0.12f, screenW * 0.7f, screenH * 0.05f));
        GUILayout.BeginHorizontal("box");

        for (i = 0; i < optionsH.Length; i++)
        {
            GUILayout.Button(optionsH[i], "Matrix", GUILayout.Width(screenW * 0.055f), GUILayout.Height(screenH * 0.045f));
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        //Draw Vertical Labels
        GUILayout.BeginArea(new Rect(screenW * 0.34f, screenH * 0.175f, screenW * 0.07f, screenH * 0.7f));
        GUILayout.BeginVertical("box");

        for (i = 0; i < optionsV.Length; i++)
        {
            GUILayout.Button(optionsV[i], "Matrix", GUILayout.Width(screenW * 0.055f), GUILayout.Height(screenH * 0.045f));
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();

        for (i = 0; i < 9; i++)
        {
            for (j = 0; j < 9; j++)
            {
                if (matches == 20 && clicks == 20)
                {
                    if (level == 1)
                        score = 3;
                    else if (level == 2)
                        score = 6;

                    highlighting = true;
                    nextStep();
                }
                else
                {
                    highlighting = false;
                    playing = true;
                    step = 1;
                }

                if (highlighting && i == j && toggle[i] == toggle[j])
                {
                    toggle[i * 9 + j] = GUI.Toggle(new Rect(screenW * (float)(0.425 + 0.061 * j), screenH * (float)(0.185 + 0.055 * i), screenW * 0.02f, screenH * 0.03f), toggle[i * 9 + j], "", "Match");
                }
                else
                {
                    toggle[i * 9 + j] = GUI.Toggle(new Rect(screenW * (float)(0.425 + 0.061 * j), screenH * (float)(0.185 + 0.055 * i), screenW * 0.02f, screenH * 0.03f), toggle[i * 9 + j], "");

                    if (GUI.changed)
                    {
                        if (toggle[i * 9 + j])
                        {
                            if (optionsH[j] == optionsV[i])
                            {	//if a correct button is turned on
                                if (!answers[i * 9 + j])
                                {
                                    matches++;
                                }
                                answers[i * 9 + j] = true;
                            }
                            else
                            {	//if an incorrect button is turned on				
                                answers[i * 9 + j] = false;
                            }
                        }
                        else
                        {						//if any button is turned off		    	 
                            if (answers[i * 9 + j])
                            { 	//if a correct button is turned off
                                matches--;
                            }
                            answers[i * 9 + j] = false;
                            continue;
                        }
                    }
                }
            }
        }

        clicks = 0;
        foreach (bool on in toggle)
        {
            if (on)
                clicks++;
        }
    }

    private void drawQuestion()
    {
        string question;
        int answer;

        if (finalQuestion % 2 == 0)
        {
            question = "\nWhich one of these panels depicts the best dot matrix?";
            answer = 1;
            image = question1;
        }
        else
        {
            question = "\nWhich one of these is the best dot matrix alignment?";
            answer = 0;
            image = question2;
        }

        GUI.Label(new Rect(screenW * 0.3f, screenH * 0.15f, screenW * 0.7f, screenH * 0.1f), "Answer the following question based on the image below");
        GUI.Label(new Rect(screenW * 0.34f, screenH * 0.2f, screenW * 0.6f, screenH * 0.5f), new GUIContent(question, image));
        GUI.Label(new Rect(screenW * 0.38f, screenH * 0.70f, screenW * 0.015f, screenH * 0.1f), "A");
        GUI.Label(new Rect(screenW * 0.38f, screenH * 0.74f, screenW * 0.015f, screenH * 0.1f), "B");
        GUI.Label(new Rect(screenW * 0.38f, screenH * 0.78f, screenW * 0.015f, screenH * 0.1f), "C");
        GUI.Label(new Rect(screenW * 0.38f, screenH * 0.82f, screenW * 0.015f, screenH * 0.1f), "D");

        for (i = 0; i < 4; i++)
        {
            if (GUI.Toggle(new Rect(screenW * 0.36f, screenH * (float)(0.7 + i * 0.04), screenW * 0.015f, screenW * 0.015f), toggle[i], ""))
            {
                toggle[0] = toggle[1] = toggle[2] = toggle[3] = false;
                toggle[i] = true;

                if (i == answer)
                {
                    GUI.Label(new Rect(screenW * 0.42f, screenH * (float)(0.70 + i * 0.04), screenW * 0.2f, screenW * 0.02f), "Correct!");
                    score = 10;
                    nextStep();
                }
                else
                {
                    GUI.Label(new Rect(screenW * 0.42f, screenH * (float)(0.70 + i * 0.04), screenW * 0.2f, screenW * 0.02f), "Incorrect! Try Again!");
                }
            }
        }
    }

    private IEnumerator endGame()
    {
        yield return new WaitForSeconds(3.0f);
        drawing = false;
        transitionVisible = true;
    }

    private void exit()
    {
        /*
        gameStarted = false;

        GameObject.Find("Main Camera").camera.enabled = true;
        this.camera.enabled = false;
         */
        Application.LoadLevel("Laboratory");
    }

    private IEnumerator nextLevel()
    {
        yield return new WaitForSeconds(4.0f);
        if (highlighting)
        {
            drawing = false;
            transitionVisible = false;
            step = 1;

            if (score == 3)
            {
                level = 2;
            }
            else if (score == 6)
            {
                level = 3;
            }
        }
    }

    private void nextStep()
    {
        if (level == 3)
        {
            step = 3;
            transitionVisible = false;
        }
        else
        {
            step = 2;
            transitionVisible = true;
        }
        playing = false;
    }
}