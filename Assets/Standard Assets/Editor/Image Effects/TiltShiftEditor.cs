// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TiltShift))]
public class TiltShiftEditor : Editor
{
    SerializedObject serObj;

    SerializedProperty focalPoint;
    SerializedProperty smoothness;
    SerializedProperty visualizeCoc;

    SerializedProperty renderTextureDivider;
    SerializedProperty blurIterations;
    SerializedProperty foregroundBlurIterations;
    SerializedProperty maxBlurSpread;
    SerializedProperty enableForegroundBlur;

    void OnEnable()
    {
        serObj = new SerializedObject(target);

        focalPoint = serObj.FindProperty("focalPoint");
        smoothness = serObj.FindProperty("smoothness");
        visualizeCoc = serObj.FindProperty("visualizeCoc");

        renderTextureDivider = serObj.FindProperty("renderTextureDivider");
        blurIterations = serObj.FindProperty("blurIterations");
        foregroundBlurIterations = serObj.FindProperty("foregroundBlurIterations");
        maxBlurSpread = serObj.FindProperty("maxBlurSpread");
        enableForegroundBlur = serObj.FindProperty("enableForegroundBlur");
    }

    public override void OnInspectorGUI()
    {
        serObj.Update();

        GameObject go = (target as TiltShift).gameObject;

        if (!go)
            return;

        if (!go.GetComponent<Camera>())
            return;

        GUILayout.Label("Current: " + go.GetComponent<Camera>().name + ", near " + go.GetComponent<Camera>().nearClipPlane + ", far: " + go.GetComponent<Camera>().farClipPlane + ", focal: " + focalPoint.floatValue, EditorStyles.miniBoldLabel);

        GUILayout.Label("Focal Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(visualizeCoc, new GUIContent("Visualize"));
        focalPoint.floatValue = EditorGUILayout.Slider("Distance", focalPoint.floatValue, go.GetComponent<Camera>().nearClipPlane, go.GetComponent<Camera>().farClipPlane);
        EditorGUILayout.PropertyField(smoothness, new GUIContent("Smoothness"));

        EditorGUILayout.Separator();

        GUILayout.Label("Background Blur", EditorStyles.boldLabel);
        renderTextureDivider.intValue = (int)EditorGUILayout.Slider("Downsample", renderTextureDivider.intValue, 1, 3);
        blurIterations.intValue = (int)EditorGUILayout.Slider("Iterations", blurIterations.intValue, 1, 4);
        EditorGUILayout.PropertyField(maxBlurSpread, new GUIContent("Max blur spread"));

        EditorGUILayout.Separator();

        GUILayout.Label("Foreground Blur", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(enableForegroundBlur, new GUIContent("Enable"));

        if (enableForegroundBlur.boolValue)
            foregroundBlurIterations.intValue = (int)EditorGUILayout.Slider("Iterations", foregroundBlurIterations.intValue, 1, 4);

        //GUILayout.Label ("Background options");
        //edgesOnly.floatValue = EditorGUILayout.Slider ("Edges only", edgesOnly.floatValue, 0.0f, 1.0f);
        //EditorGUILayout.PropertyField (edgesOnlyBgColor, new GUIContent ("Background"));    		

        serObj.ApplyModifiedProperties();
    }
}