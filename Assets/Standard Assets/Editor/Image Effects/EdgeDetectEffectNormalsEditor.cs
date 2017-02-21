//// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
//// Do test the code! You usually need to change a few small bits.

//using UnityEngine;
//using UnityEditor;
//using System.Collections;


//[ExecuteInEditMode()]

//[CustomEditor(typeof(EdgeDetectEffectNormals))]

//class EdgeDetectEffectNormalsEditor : Editor
//{
//    SerializedObject serObj;

//    SerializedProperty mode;
//    SerializedProperty sensitivityDepth;
//    SerializedProperty sensitivityNormals;

//    SerializedProperty edgesOnly;
//    SerializedProperty edgesOnlyBgColor;


//    void OnEnable()
//    {
//        serObj = new SerializedObject(target);

//        mode = serObj.FindProperty("mode");

//        sensitivityDepth = serObj.FindProperty("sensitivityDepth");
//        sensitivityNormals = serObj.FindProperty("sensitivityNormals");

//        edgesOnly = serObj.FindProperty("edgesOnly");
//        edgesOnlyBgColor = serObj.FindProperty("edgesOnlyBgColor");
//    }

//    void OnInspectorGUI()
//    {
//        serObj.Update();

//        EditorGUILayout.PropertyField(mode, new GUIContent("Mode"));

//        GUILayout.Label("Edge sensitivity");
//        EditorGUILayout.PropertyField(sensitivityDepth, new GUIContent("Depth"));
//        EditorGUILayout.PropertyField(sensitivityNormals, new GUIContent("Normals"));

//        EditorGUILayout.Separator();

//        GUILayout.Label("Background options");
//        edgesOnly.floatValue = EditorGUILayout.Slider("Edges only", edgesOnly.floatValue, 0.0f, 1.0f);
//        EditorGUILayout.PropertyField(edgesOnlyBgColor, new GUIContent("Background"));

//        serObj.ApplyModifiedProperties();
//    }
//}