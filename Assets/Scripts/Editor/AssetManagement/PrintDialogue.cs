using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PrintDialogue : EditorWindow
{
    [MenuItem("Metablast/Utility/Print Dialogue")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow<PrintDialogue>().Show();
    }

    private string _outputFilePath = "";
    private string[] _sceneNames;
    private string[] _scenePaths;

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Output File Path: ", GUILayout.ExpandWidth(false));
        _outputFilePath = GUILayout.TextField(_outputFilePath, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();


        if (GUILayout.Button("Print Dialogue"))
        {
            GetSceneNames();
            CreateOutputDirectory();
            EnumerateScenesAndPrintOutput();
        }
    }

    private void GetSceneNames()
    {
        ReadNames(out _scenePaths, out _sceneNames);
    }

    private void CreateOutputDirectory()
    {
        if (!Directory.Exists("DialogueData"))
        {
            Directory.CreateDirectory("DialogueData");
        }
        if (!Directory.Exists("DialogueData/Scenes"))
        {
            Directory.CreateDirectory("DialogueData/Scenes");
        }
    }

    private void EnumerateScenesAndPrintOutput()
    {
        //int i = 3;
        using (StreamWriter indexWriter = new StreamWriter(File.Create("DialogueData/index.html")))
        {
			indexWriter.Write("<head>");
			indexWriter.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            for (int i = 0; i < _scenePaths.Length; i++)
            {
                EditorSceneManager.OpenScene(_scenePaths[i]);
                
                string html = BuildDialogueHTML(_sceneNames[i]);
				
                if (!string.IsNullOrEmpty(html))
                {
                    indexWriter.Write("<a href=\"Scenes/" + _sceneNames[i] + ".html\">" + _sceneNames[i] + "</a><br>");
                    using (StreamWriter writer = new StreamWriter(File.Create("DialogueData/Scenes/" + _sceneNames[i] + ".html")))
                    {
                        writer.Write(html);
                    }
                }
            }
			indexWriter.Write("</head>");
        }
    }

    private string BuildDialogueHTML(string sceneName)
    {
        List<DialogueNodeComponent> rootNodes = new List<DialogueNodeComponent>();
        DialogueNodeComponent[] allDialogueNodes = (DialogueNodeComponent[])Resources.FindObjectsOfTypeAll(typeof(DialogueNodeComponent));

        ShowTransmission[] allTransmissions = (ShowTransmission[])Resources.FindObjectsOfTypeAll(typeof(ShowTransmission));

        foreach (DialogueNodeComponent dialogueNode in allDialogueNodes)
        {
            if (!dialogueNode.transform.parent.GetComponent<DialogueNodeComponent>() && !(dialogueNode is DialogueJumpNode))
            {
                rootNodes.Add(dialogueNode);
            }
        }

        if (rootNodes.Count == 0 && allTransmissions.Length == 0)
        {
            return "";
        }


        string html = "<head>";
		html += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">";
		html += "<H3>" + sceneName + "</H3>";

        HashSet<DialogueNodeComponent> encounteredNodes = new HashSet<DialogueNodeComponent>();

        foreach (DialogueNodeComponent node in rootNodes)
        {
            html += GetHTMLForDialogueNode(node, encounteredNodes);
        }

        foreach (ShowTransmission transmission in allTransmissions)
        {
            if (transmission.Duration == 0)
            {
                continue;
            }
            html += "<div style=\"margin-left:25px;border:2px solid;margin-bottom:25px;padding-left:20px\";>";
            html += "<p>";
            html += GetHTMLForTransmissionText(transmission);
            html += "</p>";
            html += "</div>";
        }
		
		html += "</head>";
        return html;
    }

    private string GetHTMLForDialogueNode(DialogueNodeComponent nodeComponent, HashSet<DialogueNodeComponent> encounteredNodes)
    {
        if (encounteredNodes.Contains(nodeComponent))
        {
            return "<H4 style=\"margin-bottom:5px\";>Reference: " + nodeComponent.name + "</H4>\n";
        }
        else
        {
            encounteredNodes.Add(nodeComponent);
        }

        string toReturn = "<H4 style=\"margin-bottom:5px\";>" + nodeComponent.name + "</H4>\n";

        toReturn += "<div style=\"margin-left:25px;border:2px solid;margin-bottom:25px;padding-left:20px\";>";
        toReturn += "<p>";

        toReturn += "<body><i>Sender:</i> " + nodeComponent.DialogueData.Sender + "</body><br>\n";
        toReturn += "<body><i>Message:</i> " + nodeComponent.DialogueData.Message + "</body>\n";
        toReturn += "<br>\n";


        DialogueNodeComponent successor = null;
        List<DialogueTransitionNodeComponent> transitions = new List<DialogueTransitionNodeComponent>();

        foreach (Transform child in nodeComponent.transform)
        {
            transitions.AddRange(child.GetComponents<DialogueTransitionNodeComponent>());
            successor = child.GetComponent<DialogueNodeComponent>() ?? successor;
        }

        if (successor)
        {
            toReturn += "<div style=\"margin-left:25px\";>";
            toReturn += "<p>";
            if (successor is DialogueJumpNode)
            {
                if ((successor as DialogueJumpNode).JumpTarget)
                {
                    toReturn += GetHTMLForDialogueNode((successor as DialogueJumpNode).JumpTarget, encounteredNodes);
                }
            }
            else
            {
                toReturn += GetHTMLForDialogueNode(successor, encounteredNodes);
            }
            toReturn += "</p>";
            toReturn += "</div>";
        }

        foreach (DialogueTransitionNodeComponent transition in transitions)
        {
            string text = transition.DialogueTransitionData.TransitionText;
            toReturn += "<H4 style=\"margin-bottom:5px\";>Transition (" + text + ")</H4>";

            DialogueNodeComponent transitionSucessor = null;
            foreach (Transform child in transition.transform)
            {
                transitionSucessor = child.GetComponent<DialogueNodeComponent>() ?? transitionSucessor;
            }
            toReturn += GetHTMLForDialogueNode(transitionSucessor, encounteredNodes);
        }
        toReturn += "</p>";
        toReturn += "</div>";

        return toReturn;
    }

    private string GetHTMLForTransmissionText(ShowTransmission transmission)
    {
        string toReturn = "<H4 style=\"margin-bottom:5px\";>" + "Transmission" + "</H4>\n";

        toReturn += "<body><i>Duration:</i> " + transmission.Duration.ToString() + "</body><br>\n";

        toReturn += "<body><i>Message:</i> " + transmission.TransmissionText ?? "" + "</body>\n";
        toReturn += "<br>\n";

        return toReturn;
    }

    /*
    void ShowDialogueGUI(DialogueNodeComponent dialogueNode, int depth)
    {
        GUILayout.Label(depth.ToString() + ": Dialogue Node: " + dialogueNode.name);
        GUILayout.Label(dialogueNode.DialogueData.Sender);
        GUILayout.BeginHorizontal();
        GUILayout.Space(25);
        GUILayout.BeginVertical();
        GUILayout.Label(dialogueNode.DialogueData.Message);

        foreach (Transform child in dialogueNode.transform)
        {
            DialogueNodeComponent childDialogue = child.GetComponent<DialogueNodeComponent>();

            if (childDialogue)
            {
                ShowDialogueGUI(childDialogue, depth + 1);
            }
        }

        foreach (DialogueTransitionNodeComponent transition in dialogueNode.DialogueTransitions)
        {
            GUILayout.Label("Transition: " + transition.DialogueTransitionData.TransitionText);
            ShowDialogueGUI(transition.DialogueNodeComponent, depth + 1);
        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }*/

    private static void ReadNames(out string[] scenePaths, out string[] sceneNames)
    {
        List<string> scenePathsList = new List<string>();
        List<string> sceneNameList = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {

                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                scenePathsList.Add(S.path);
                sceneNameList.Add(name);
            }
        }

        scenePaths = scenePathsList.ToArray();
        sceneNames = sceneNameList.ToArray();
    }
}

