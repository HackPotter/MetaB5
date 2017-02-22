#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System;

public enum PopupType { Message, YesNo, Controls, Tutorial, Pause }

public class Popup : MonoBehaviour
{
    public static bool visible;
    public GUISkin skin;
    public Action YesEvent;
    public Action NoEvent;
    public Action CloseEvent;

    private string message;
    private PopupType type;

    public Texture2D blackBackground;
    public Texture2D messageBoxTexture;
    public Texture2D controlsBoxTexture;
    public Texture2D pauseBoxTexture;

    private float currentX;
    private float currentY;
    private float xPadding = 0;
    private float yPadding = 0;
    private float paddedWidth;
    private float paddedHeight;
    private float widthRatio;
    private float heightRatio;

    private Rect FinalTutorialRect;
    private Rect TutorialRect;
    private Rect TutorialMessageRect;
    private string TutorialMessageText;

    private Texture2D backgroundMenu;

    void Start()
    {
        visible = false;
        TutorialMessageText = "";
        //useGUILayout = false;
    }

    void OnGUI()
    {
        if (visible)
        {
            GUI.depth = 0;
            GUI.skin = skin;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBackground);
            if (type != PopupType.Tutorial)
            {
                Time.timeScale = 0.0f;
            }
            //GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height), menuTexture);
            //GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, Vector3 (Screen.width / (backgroundMenu.width * 1.0f), Screen.height / (backgroundMenu.height * 1.0f), 1));

            if (type == PopupType.Message || type == PopupType.YesNo)
            {
                paddedWidth = Screen.width * 0.1f;
                paddedHeight = Screen.height * 0.25f;

                widthRatio = Screen.width / (1.0f * messageBoxTexture.width);
                heightRatio = Screen.height / (1.0f * messageBoxTexture.height);

                //draw background
                GUI.DrawTexture(new Rect(paddedWidth, paddedHeight, Screen.width - (2 * paddedWidth), Screen.height - (2 * paddedHeight)), messageBoxTexture);

                currentX = paddedWidth + ((260 / (1.0f * messageBoxTexture.width)) * (Screen.width - (2 * paddedWidth)));
                currentY = paddedHeight + ((115 / (1.0f * messageBoxTexture.height)) * (Screen.height - (2 * paddedHeight)));
                GUI.Label(new Rect(currentX, currentY, (700 * widthRatio) - (2 * ((700 * widthRatio) * xPadding)), (145 * heightRatio) - (2 * ((145 * heightRatio) * yPadding))), message);

                currentX = paddedWidth + ((1110 / (1.0f * messageBoxTexture.width)) * (Screen.width - (2 * paddedWidth)));
                currentY = paddedHeight + ((300 / (1.0f * messageBoxTexture.height)) * (Screen.height - (2 * paddedHeight)));

                //Determine what buttons we need to display (Yes/No or Close)
                if (type == PopupType.Message)
                    finishMessageBox();
                else if (type == PopupType.YesNo)
                    finishYesNoBox();
            }
            else if (type == PopupType.Tutorial)
            {
                GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / (backgroundMenu.width * 1.0f), Screen.height / (backgroundMenu.height * 1.0f), 1));
                GUI.depth = 0;
                GUI.Box(TutorialRect, "", "TutorialBox");
                GUI.Label(TutorialMessageRect, TutorialMessageText, "TutorialLabel");

                if (TutorialMessageText != "")
                {
                    if (GUI.Button(new Rect(TutorialMessageRect.x + TutorialMessageRect.width / 2 - 50, TutorialMessageRect.y + FinalTutorialRect.height - (FinalTutorialRect.height - TutorialMessageRect.height), 100, 50), "Close"))
                    {
                        visible = false;
                        Time.timeScale = 1.0f;
                        if (CloseEvent != null)
                            CloseEvent();
                    }
                }
            }
            else if (type == PopupType.Controls)
            {
                paddedWidth = Screen.width * xPadding;
                paddedHeight = Screen.height * yPadding;

                float widthModifier = (Screen.width - (2 * paddedWidth));
                float heightModifier = (Screen.height - (2 * paddedHeight));

                widthRatio = Screen.width / (1.0f * controlsBoxTexture.width);
                heightRatio = Screen.height / (1.0f * controlsBoxTexture.height);

                float paddedWidthRatio = widthRatio * xPadding;
                float paddedHeightRatio = heightRatio * yPadding;

                GUI.DrawTexture(new Rect(paddedWidth, paddedHeight, widthModifier, heightModifier), controlsBoxTexture);

                currentX = paddedWidth + ((2260 / (1.0f * controlsBoxTexture.width)) * (Screen.width - (2 * paddedWidth)));
                currentY = paddedHeight + ((1273 / (1.0f * controlsBoxTexture.height)) * (Screen.height - (2 * paddedHeight)));
                if (GUI.Button(new Rect(currentX, currentY, (75 * widthRatio) - (2 * ((75 * widthRatio) * xPadding)), (75 * heightRatio) - (2 * 75 * heightRatio) * yPadding), "", "backButton"))
                {
                    visible = false;
                    Time.timeScale = 1.0f;
                    CloseEvent();
                }
            }
            else
            {
                paddedWidth = Screen.width * 0.25f;
                paddedHeight = Screen.height * 0.125f;

                float widthModifier = (Screen.width - (2 * paddedWidth));
                float heightModifier = (Screen.height - (2 * paddedHeight));

                widthRatio = Screen.width / (1.0f * messageBoxTexture.width);
                heightRatio = Screen.height / (1.0f * messageBoxTexture.height);
                GUI.DrawTexture(new Rect(paddedWidth, paddedHeight, widthModifier, heightModifier), pauseBoxTexture);

                currentX = paddedWidth + ((1110 / (1.0f * pauseBoxTexture.width)) * (Screen.width - (2 * paddedWidth)));

                //currentX *= 0.5f;
                currentY = paddedHeight + heightModifier * 0.38f;

                skin.button.fontSize = (int)(Screen.height * 0.02f);
                if (GUI.Button(new Rect(currentX, currentY, (160 * widthRatio) - (2 * ((110 * widthRatio) * xPadding)), (25 * heightRatio) - (2 * 25 * heightRatio) * yPadding), "Resume"))
                {
                    visible = false;
                    Time.timeScale = 1.0f;
                }

                currentY += ((25 * heightRatio) - (2 * 25 * heightRatio) * yPadding) * 1.5f;

                currentY += ((25 * heightRatio) - (2 * 25 * heightRatio) * yPadding) * 1.5f;
                if (GUI.Button(new Rect(currentX, currentY, (160 * widthRatio) - (2 * ((110 * widthRatio) * xPadding)), (25 * heightRatio) - (2 * 25 * heightRatio) * yPadding), "Exit"))
                {
                    visible = false;
                    Time.timeScale = 1.0f;
                    CloseEvent();
                }
            }
        }
    }

    private void setType(PopupType type)
    {
        this.type = type;
    }

    private void setMessage(string message)
    {
        this.message = message;
    }

    public void Show(PopupType type, string message, Texture2D scaleToBackground)
    {
        visible = true;
        backgroundMenu = scaleToBackground;
        FinalTutorialRect = new Rect(backgroundMenu.width / 2 - 400, backgroundMenu.height / 2 - 250, 800, 500);
        TutorialMessageRect = new Rect(FinalTutorialRect.x, FinalTutorialRect.y, 800, 400);
        setType(type);
        setMessage(message);

        if (type == PopupType.Tutorial)
        {
            TutorialMessageText = "";
            iTween.ValueTo(gameObject, iTween.Hash("from", new Rect(backgroundMenu.width / 2, backgroundMenu.height / 2, 0, 0), "to", FinalTutorialRect, "delay", 0.0f, "time", 1.0f, "easetype", iTween.EaseType.easeInOutSine, "onupdate", "expandTutorialRect", "oncomplete", "DisplayTutorialMessage", "ignoretimescale", true));
        }
    }

    private void expandTutorialRect(Rect input)
    {
        TutorialRect = input;
    }

    private void DisplayTutorialMessage()
    {
        TutorialMessageText = message;
        /*
        TextTimerTicks = 0;
        TextTimer.ResetTimer();
        TextTimer.OnTick = TextTimerTick;
        TextTimer.StartTimer(TextTimerSpeed, Message.Length);*/
    }

    private void finishMessageBox()
    {
        currentX = paddedWidth + ((1110 / (1.0f * messageBoxTexture.width)) * (Screen.width - (2 * paddedWidth)));
        currentY = paddedHeight + ((300 / (1.0f * messageBoxTexture.height)) * (Screen.height - (2 * paddedHeight)));
        if (GUI.Button(new Rect(currentX, currentY, (110 * widthRatio) - (2 * ((110 * widthRatio) * xPadding)), (25 * heightRatio) - (2 * 25 * heightRatio) * yPadding), "Close"))
        {
            visible = false;
            Time.timeScale = 1.0f;
            CloseEvent();
        }
    }

    private void finishYesNoBox()
    {
        currentX = paddedWidth + ((950 / (1.0f * messageBoxTexture.width)) * (Screen.width - (2 * paddedWidth)));
        currentY = paddedHeight + ((300 / (1.0f * messageBoxTexture.height)) * (Screen.height - (2 * paddedHeight)));
        if (GUI.Button(new Rect(currentX, currentY, (110 * widthRatio) - (2 * ((110 * widthRatio) * 0)), (25 * heightRatio) - (2 * 25 * heightRatio) * 0), "Yes"))
        {
            visible = false;
            Time.timeScale = 1.0f;
            YesEvent();
        }

        currentX = paddedWidth + ((1110 / (1.0f * messageBoxTexture.width)) * (Screen.width - (2 * paddedWidth)));
        currentY = paddedHeight + ((300 / (1.0f * messageBoxTexture.height)) * (Screen.height - (2 * paddedHeight)));
        if (GUI.Button(new Rect(currentX, currentY, (110 * widthRatio) - (2 * ((110 * widthRatio) * 0)), (25 * heightRatio) - (2 * 25 * heightRatio) * 0), "No"))
        {
            visible = false;
            Time.timeScale = 1.0f;
            NoEvent();
        }
    }

    public void DoNothing() { }
}