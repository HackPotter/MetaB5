// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class MiniGameLoader : MonoBehaviour
{
    public string SceneToLoad;
    public Texture2D Logo;

    private bool visible;

    void Start()
    {
        visible = false;
        //EditorBuildSettings.scenes
    }

    void OnGUI()
    {
        if (visible)
        {
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / (1600.0f), Screen.height / (900.0f), 1));

            //layout start
            GUI.BeginGroup(new Rect(509, 380, 582, 500));

            //the menu background box
            GUI.Box(new Rect(0, 0, 582, 500), "");

            //logo picture
            GUI.DrawTexture(new Rect(30, 10, 512, 317), Logo);

            if (GUI.Button(new Rect(201, 351, 180, 40), "Start Game"))
            {
            }

            //quit button
            if (GUI.Button(new Rect(201, 425, 180, 40), "Cancel"))
            {
                visible = false;
            }

            //layout end
            GUI.EndGroup();
        }
    }

    public void Show()
    {
        visible = true;
    }
}