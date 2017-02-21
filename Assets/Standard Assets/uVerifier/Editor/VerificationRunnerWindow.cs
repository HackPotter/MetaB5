using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// The EditorWindow for uVerifier.
/// </summary>
public class VerificationRunnerWindow : EditorWindow
{
    private List<FieldViolation> fieldViolations;
    private Vector2 _scrollPosition;

    private List<IConditionVerifier> _conditionVerifiers;
    private bool[] _conditionVerifierToggles;

    [MenuItem("Metablast/Testing/uVerifier")]
    private static void OpenVerifier()
    {
        var window = ScriptableObject.CreateInstance<VerificationRunnerWindow>();
        window.Initialize();
        window.Show();
    }

    /// <summary>
    /// Initializes the VerificationRunnerWindow.
    /// <br/>
    /// Searches all Assemblies currently loaded in the active AppDomain for types implementing IConditionVerifier.
    /// </summary>
    private void Initialize()
    {
        Type[] types = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetTypes()).SelectMany(x => x).Where(x => x.GetInterfaces().Contains(typeof(IConditionVerifier))).ToArray();

        _conditionVerifiers = new List<IConditionVerifier>();

        foreach (Type type in types)
        {
            if (type.IsAbstract)
            {
                continue;
            }
            IConditionVerifier conditionVerifier = Activator.CreateInstance(type) as IConditionVerifier;
            if (conditionVerifier != null)
            {
                _conditionVerifiers.Add(conditionVerifier);
            }
        }

        _conditionVerifierToggles = new bool[_conditionVerifiers.Count];
    }

    private void OnGUI()
    {
        GUILayout.Label("Unity Verifier");

        if (_conditionVerifiers == null)
        {
            GUILayout.Label("Searching for Condition Verifiers...");
            return;
        }

        int i = 0;
        foreach (IConditionVerifier conditionVerifier in _conditionVerifiers)
        {
            _conditionVerifierToggles[i] = EditorGUILayout.Toggle(conditionVerifier.GetType().ToString(), _conditionVerifierToggles[i]);
            i++;
        }

        if (GUILayout.Button("Run Verifier"))
        {
            fieldViolations = RunVerifier();
        }

        if (fieldViolations == null)
        {
            GUILayout.Label("Press Run Verifier button to scan scene for violations!");
            return;
        }

        if (fieldViolations.Count == 0)
        {
            GUILayout.Label("No violations found!");
        }

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        foreach (var fieldViolation in fieldViolations)
        {
            GUILayout.Space(10);
            GUILayout.Label(fieldViolation.Message);

            GUILayout.Space(5);
            GUILayout.Label("Object Containing Violation");
            EditorGUILayout.ObjectField(fieldViolation.ViolatingObject, typeof(UnityEngine.Object), false);


            string label = "Target of Violation: ";
            GUILayout.Label(label);
            EditorGUILayout.ObjectField(fieldViolation.Target as UnityEngine.Object, typeof(UnityEngine.Object), false);

            GUILayout.Space(10);
        }
        GUILayout.EndScrollView();

    }

    private List<FieldViolation> RunVerifier()
    {
        GameObject[] gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<FieldViolation> fieldViolations = new List<FieldViolation>();

        foreach (IConditionVerifier conditionVerifier in _conditionVerifiers)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject != null)
                {
                    fieldViolations.AddRange(conditionVerifier.VerifyCondition(gameObject));
                }
            }
        }

        return fieldViolations;
    }

}
#endif