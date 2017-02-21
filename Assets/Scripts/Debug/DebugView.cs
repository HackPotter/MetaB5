using UnityEngine;


public class DebugView : MonoBehaviour
{
    private bool _showDebugView = false;
    private bool _showPlayerData = false;
    private bool _showSessionData = false;
    private bool _showPersistentData = false;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 120, 350, 600));
        if (_showDebugView)
        {
            _showDebugView = GUILayout.Button("Hide Debug", GUILayout.ExpandWidth(false)) ? !_showDebugView : _showDebugView;
        }
        else
        {
            _showDebugView = GUILayout.Button("Show Debug", GUILayout.ExpandWidth(false)) ? !_showDebugView : _showDebugView;
        }

        if (!_showDebugView)
        {
            GUILayout.EndArea();
            return;
        }

        GUILayout.BeginHorizontal();
        _showPlayerData = GUILayout.Button("Show Player Data", GUILayout.ExpandWidth(false)) ? !_showPlayerData : _showPlayerData;
        _showSessionData = GUILayout.Button("Show Session Data", GUILayout.ExpandWidth(false)) ? !_showSessionData : _showSessionData;
        _showPersistentData = GUILayout.Button("Show Persistent Data", GUILayout.ExpandWidth(false)) ? !_showPersistentData : _showPersistentData;
        GUILayout.EndHorizontal();

        if (_showPlayerData)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Guid: " + GameContext.Instance.Player.UserGuid);
            GUILayout.Label("Tool: " + GameContext.Instance.Player.ActiveTool);
            GUILayout.Label("ATP: " + GameContext.Instance.Player.ATP);
            GUILayout.Label("NADPH: " + GameContext.Instance.Player.NADPH);
            GUILayout.Label("O2: " + GameContext.Instance.Player.O2);
            GUILayout.EndVertical();
        }

        if (_showSessionData)
        {
            string textFieldString = "";
            foreach (var dataVal in GameContext.Instance.Player.SessionStorage)
            {
                textFieldString += dataVal.Key + " : " + dataVal.Value + "\n";
            }

            GUILayout.TextArea(textFieldString);
        }

        if (_showPersistentData)
        {
            string textFieldString = "";
            foreach (var dataVal in GameContext.Instance.Player.PersistentStorage)
            {
                textFieldString += dataVal.Key + " : " + dataVal.Value + "\n";
            }

            GUILayout.TextArea(textFieldString);
        }

        GUILayout.EndArea();
    }
}

