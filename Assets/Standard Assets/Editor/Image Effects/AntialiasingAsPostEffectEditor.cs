// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AntialiasingAsPostEffect))]

public class AntialiasingAsPostEffectEditor : Editor
{
    SerializedObject serObj;

    SerializedProperty mode;

    SerializedProperty showGeneratedNormals;
    SerializedProperty offsetScale;
    SerializedProperty blurRadius;
    SerializedProperty dlaaSharp;

    void OnEnable()
    {
        serObj = new SerializedObject(target);

        mode = serObj.FindProperty("mode");

        showGeneratedNormals = serObj.FindProperty("showGeneratedNormals");
        offsetScale = serObj.FindProperty("offsetScale");
        blurRadius = serObj.FindProperty("blurRadius");
        dlaaSharp = serObj.FindProperty("dlaaSharp");
    }

    public override void OnInspectorGUI()
    {
        serObj.Update();

        GUILayout.Label("Various luminance based fullscreen Antialiasing techniques", EditorStyles.miniBoldLabel);

        EditorGUILayout.PropertyField(mode, new GUIContent("AA Technique"));

        if (mode.enumValueIndex == (int)AAMode.NFAA)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(offsetScale, new GUIContent("Edge Detect Ofs"));
            EditorGUILayout.PropertyField(blurRadius, new GUIContent("Blur Radius"));
            EditorGUILayout.PropertyField(showGeneratedNormals, new GUIContent("Show Normals"));
        }
        else if (mode.enumValueIndex == (int)AAMode.DLAA)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(dlaaSharp, new GUIContent("Sharp"));
        }

        serObj.ApplyModifiedProperties();
    }
}