/*
 * NGImmediateWindow.cs
 * Copyright (c) 2012 Nick Gravelyn
*/

using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Provides an editor window for quickly compiling and running snippets of code.
/// </summary>
public class NGImmediateWindow : EditorWindow
{
    // positions for the two scroll views
    private Vector2 scrollPos;
    private Vector2 errorScrollPos;

    // the script text string
    private string scriptText = string.Empty;

    // stored away compiler errors (if any) and the compiled method
    private CompilerErrorCollection compilerErrors = null;
    private MethodInfo compiledMethod = null;
    
    void OnGUI()
    {
        // make a scroll view for the text area
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        // place a text area in the scroll view
        string newScriptText = EditorGUILayout.TextArea(scriptText, GUILayout.ExpandHeight(true));

        // if the script updated save the script and remove the compiled method as it's no longer valid
        if (scriptText != newScriptText)
        {
            scriptText = newScriptText;
            compiledMethod = null;
        }

        EditorGUILayout.EndScrollView();

        // setup the compile/run button
        if (GUILayout.Button(compiledMethod == null ? "Compile + Run" : "Run"))
        {
            // if the method is already compiled or if we successfully compile the script text, invoke the method
            if (compiledMethod != null || NGCompiler.CompileCSharpImmediateSnippet(scriptText, out compilerErrors, out compiledMethod))
            {
                compiledMethod.Invoke(null, null);
            }
        }

        // if we have any errors, we display them in their own scroll view
        if (compilerErrors != null && compilerErrors.Count > 0)
        {
            // build up one string for errors and one for warnings
            StringBuilder errorString = new StringBuilder();
            StringBuilder warningString = new StringBuilder();

            foreach (CompilerError e in compilerErrors)
            {
                if (e.IsWarning)
                {
                    warningString.AppendFormat("Warning on line {0}: {1}\n", e.Line, e.ErrorText);
                }
                else
                {
                    errorString.AppendFormat("Error on line {0}: {1}\n", e.Line, e.ErrorText);
                }
            }

            // remove trailing new lines from both strings
            if (errorString.Length > 0)
            {
                errorString.Length -= 2; 
            }

            if (warningString.Length > 0)
            {
                warningString.Length -= 2; 
            }

            // make a simple UI layout with a scroll view and some labels
            GUILayout.Label("Errors and warnings:");
            errorScrollPos = EditorGUILayout.BeginScrollView(errorScrollPos, GUILayout.MaxHeight(100));

            if (errorString.Length > 0)
            {
                GUILayout.Label(errorString.ToString());
            }

            if (warningString.Length > 0)
            {
                GUILayout.Label(warningString.ToString());
            }

            EditorGUILayout.EndScrollView();
        }
    }

    /// <summary>
    /// Fired when the user chooses the menu item
    /// </summary>
    [MenuItem("Window/Immediate %#I")]
    static void Init()
    {
        // get the window, show it, and give it focus
        var window = EditorWindow.GetWindow<NGImmediateWindow>("Immediate");
        window.Show();
        window.Focus();
    }
}