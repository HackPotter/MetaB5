//// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
//// Do test the code! You usually need to change a few small bits.

//using UnityEngine;
//using UnityEditor;
//using System.Collections;

//[CustomEditor(typeof(ColorCorrectionCurves))]
//public class ColorCorrectionCurvesEditor : Editor
//{
//    SerializedObject serObj;

//    SerializedProperty mode;

//    SerializedProperty redChannel;
//    SerializedProperty greenChannel;
//    SerializedProperty blueChannel;

//    SerializedProperty useDepthCorrection;

//    SerializedProperty depthRedChannel;
//    SerializedProperty depthGreenChannel;
//    SerializedProperty depthBlueChannel;

//    SerializedProperty zCurveChannel;

//    SerializedProperty selectiveCc;
//    SerializedProperty selectiveFromColor;
//    SerializedProperty selectiveToColor;

//    private bool applyCurveChanges = false;

//    void OnEnable()
//    {
//        serObj = new SerializedObject(target);

//        mode = serObj.FindProperty("mode");

//        redChannel = serObj.FindProperty("redChannel");
//        greenChannel = serObj.FindProperty("greenChannel");
//        blueChannel = serObj.FindProperty("blueChannel");

//        useDepthCorrection = serObj.FindProperty("useDepthCorrection");

//        zCurveChannel = serObj.FindProperty("zCurve");

//        depthRedChannel = serObj.FindProperty("depthRedChannel");
//        depthGreenChannel = serObj.FindProperty("depthGreenChannel");
//        depthBlueChannel = serObj.FindProperty("depthBlueChannel");

//        if (redChannel.animationCurveValue.length == 0)
//            redChannel.animationCurveValue = new AnimationCurve(new Keyframe(0, 0.0f, 1.0f, 1.0f), new Keyframe(1, 1.0f, 1.0f, 1.0f));
//        if (greenChannel.animationCurveValue.length == 0)
//            greenChannel.animationCurveValue = new AnimationCurve(new Keyframe(0, 0.0f, 1.0f, 1.0f), new Keyframe(1, 1.0f, 1.0f, 1.0f));
//        if (blueChannel.animationCurveValue.length == 0)
//            blueChannel.animationCurveValue = new AnimationCurve(new Keyframe(0, 0.0f, 1.0f, 1.0f), new Keyframe(1, 1.0f, 1.0f, 1.0f));

//        if (depthRedChannel.animationCurveValue.length == 0)
//            depthRedChannel.animationCurveValue = new AnimationCurve(new Keyframe(0, 0.0f, 1.0f, 1.0f), new Keyframe(1, 1.0f, 1.0f, 1.0f));
//        if (depthGreenChannel.animationCurveValue.length == 0)
//            depthGreenChannel.animationCurveValue = new AnimationCurve(new Keyframe(0, 0.0f, 1.0f, 1.0f), new Keyframe(1, 1.0f, 1.0f, 1.0f));
//        if (depthBlueChannel.animationCurveValue.length == 0)
//            depthBlueChannel.animationCurveValue = new AnimationCurve(new Keyframe(0, 0.0f, 1.0f, 1.0f), new Keyframe(1, 1.0f, 1.0f, 1.0f));

//        if (zCurveChannel.animationCurveValue.length == 0)
//            zCurveChannel.animationCurveValue = new AnimationCurve(new Keyframe(0, 0.0f, 1.0f, 1.0f), new Keyframe(1, 1.0f, 1.0f, 1.0f));

//        serObj.ApplyModifiedProperties();

//        selectiveCc = serObj.FindProperty("selectiveCc");
//        selectiveFromColor = serObj.FindProperty("selectiveFromColor");
//        selectiveToColor = serObj.FindProperty("selectiveToColor");
//    }

//    void CurveGui(string name, SerializedProperty animationCurve, Color color)
//    {
//        // @NOTE: EditorGUILayout.CurveField is buggy and flickers, using PropertyField for now
//        //animationCurve.animationCurveValue = EditorGUILayout.CurveField (GUIContent (name), animationCurve.animationCurveValue, color, Rect (0.0f,0.0f,1.0f,1.0f));
//        EditorGUILayout.PropertyField(animationCurve, new GUIContent(name));
//        if (GUI.changed)
//            applyCurveChanges = true;
//    }

//    void BeginCurves()
//    {
//        applyCurveChanges = false;
//    }

//    void ApplyCurves()
//    {
//        if (applyCurveChanges)
//        {
//            serObj.ApplyModifiedProperties();
//            ((ColorCorrectionCurves)serObj.targetObject).gameObject.SendMessage("UpdateTextures");
//        }
//    }

//    void OnInspectorGUI()
//    {
//        serObj.Update();

//        GUILayout.Label("Curves to tweak colors. Advanced separates fore- and background.", EditorStyles.miniBoldLabel);

//        EditorGUILayout.PropertyField(mode, new GUIContent("Mode"));

//        GUILayout.Label("Curves", EditorStyles.boldLabel);

//        BeginCurves();

//        CurveGui("Red", redChannel, Color.red);
//        CurveGui("Blue", blueChannel, Color.blue);
//        CurveGui("Green", greenChannel, Color.green);

//        EditorGUILayout.Separator();

//        if (mode.intValue > 0)
//            useDepthCorrection.boolValue = true;
//        else
//            useDepthCorrection.boolValue = false;

//        if (useDepthCorrection.boolValue)
//        {
//            CurveGui("Red (depth)", depthRedChannel, Color.red);
//            CurveGui("Blue (depth)", depthBlueChannel, Color.blue);
//            CurveGui("Green (depth)", depthGreenChannel, Color.green);
//            EditorGUILayout.Separator();
//            CurveGui("Blend Curve", zCurveChannel, Color.grey);
//        }

//        if (mode.intValue > 0)
//        {
//            EditorGUILayout.Separator();
//            GUILayout.Label("Selective Color Correction", EditorStyles.boldLabel);
//            EditorGUILayout.PropertyField(selectiveCc, new GUIContent("Enable"));
//            if (selectiveCc.boolValue)
//            {
//                EditorGUILayout.PropertyField(selectiveFromColor, new GUIContent("Key"));
//                EditorGUILayout.PropertyField(selectiveToColor, new GUIContent("Target"));
//            }
//        }

//        ApplyCurves();

//        if (!applyCurveChanges)
//            serObj.ApplyModifiedProperties();
//    }
//}
