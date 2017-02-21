using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class EventEditor : EditorWindow
{
    [MenuItem("Metablast/Suite/Event Editor")]
    private static void CreateWindow()
    {
        EventEditor window = EditorWindow.GetWindow<EventEditor>();
        window.Show();
    }

    private enum CurrentTab
    {
        SymbolTable,
        Overview,
        Editor,
    }

    private EventEditorContext _eventEditorContext;
    private OverviewEditorContext _overviewContext;

    // Window State
    private CurrentTab _currentTab = CurrentTab.Overview;

    // Tab views
    private VariableEditorViewTab _variableView = new VariableEditorViewTab();
    private TriggerEditorOverviewTab _overviewTab = new TriggerEditorOverviewTab();
    private TriggerEditorView _triggerEditor = new TriggerEditorView();

    //private bool _hasInitialized = false;

    void OnFocus()
    {
    }

    void OnGUI()
    {
        if (Application.isPlaying)
        {
            //GUILayout.Label("Unable to edit triggers while game is playing...");
            //return;
        }
        Initialize();

        Rect leftFrame = new Rect(0, 0, 300, this.position.height);
        Rect rightFrame = new Rect(301, 0, this.position.width - 300, this.position.height);

        GUILayout.BeginArea(leftFrame, GUI.skin.window);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Triggers", (_currentTab == CurrentTab.Overview ? GUI.skin.box : GUI.skin.button), GUILayout.ExpandWidth(true)))
        {
            _currentTab = CurrentTab.Overview;
        }
        if (GUILayout.Button("Variables", _currentTab == CurrentTab.SymbolTable ? GUI.skin.box : GUI.skin.button, GUILayout.ExpandWidth(true)))
        {
            _currentTab = CurrentTab.SymbolTable;
        }
        GUILayout.EndHorizontal();
        if (_currentTab == CurrentTab.Overview)
        {
            _overviewTab.Draw();
        }
        else if (_currentTab == CurrentTab.SymbolTable)
        {
            _variableView.Draw();
        }
        GUILayout.EndArea();
        GUILayout.BeginArea(rightFrame, GUI.skin.window);
        _triggerEditor.Draw();
        GUILayout.EndArea();

        if (Event.current.type == EventType.MouseDown)
        {
            GUI.FocusControl(null);
            _eventEditorContext.Repaint = true;
        }

        if (_eventEditorContext.Repaint)
        {
            Repaint();
            _eventEditorContext.Repaint = false;
        }
    }

    void OnProjectChange()
    {
        if (Application.isPlaying)
        {
            return;
        }
        Initialize();
        Repaint();
    }

    void OnHierarchyChange()
    {
        if (Application.isPlaying)
        {
            return;
        }
        Repaint();
    }

    void OnSelectionChange()
    {
        Initialize();
        Trigger selectedTrigger = null;
        Trigger[] selectedTriggers = Selection.activeGameObject.GetComponentsInParent<Trigger>(true);
        if (selectedTriggers.Length != 0)
            selectedTrigger = selectedTriggers[0];

        if (selectedTrigger && selectedTrigger != _eventEditorContext.SelectedTrigger)
        {
            _eventEditorContext.SelectTrigger(selectedTrigger);
        }
        Repaint();
    }

    public void Initialize()
    {
        if (_overviewContext == null)
        {
            _overviewContext = new OverviewEditorContext();
        }
        if (_eventEditorContext == null || _eventEditorContext.TriggerRoot == null)
        {
            TriggerRoot triggerRoot = GameObject.FindObjectOfType<TriggerRoot>();
            if (!triggerRoot)
            {
                triggerRoot = (new GameObject("Events")).AddComponent<TriggerRoot>();
            }
            GlobalSymbolTableAccessor accessor = triggerRoot.GetComponent<GlobalSymbolTableAccessor>();
            if (!accessor.GlobalSymbolTable)
            {
                accessor.GlobalSymbolTable = ScriptableObject.CreateInstance<GlobalSymbolTable>();
            }

            List<Trigger> triggers = new List<Trigger>();
            PopulateTriggerList(triggerRoot.transform, triggers);

            _eventEditorContext = new EventEditorContext(this, _overviewContext, triggerRoot, triggers, accessor.GlobalSymbolTable);

            _variableView.Context = _eventEditorContext;
            _overviewTab.Context = _eventEditorContext;
            _triggerEditor.Context = _eventEditorContext;
            return;
        }

        if (Selection.activeGameObject)
        {
            TriggerRoot[] roots = Selection.activeGameObject.GetComponentsInParent<TriggerRoot>(true);
            TriggerRoot selectedRoot = null;
            if (roots.Length != 0)
                selectedRoot = roots[0];

            if (selectedRoot)
            {
                if (selectedRoot != _eventEditorContext.TriggerRoot)
                {
                    List<Trigger> triggers = new List<Trigger>();
                    PopulateTriggerList(selectedRoot.transform, triggers);

                    GlobalSymbolTableAccessor accessor = selectedRoot.GetComponent<GlobalSymbolTableAccessor>();
                    accessor.hideFlags = HideFlags.None;
                    if (!accessor.GlobalSymbolTable)
                    {
                        accessor.GlobalSymbolTable = ScriptableObject.CreateInstance<GlobalSymbolTable>();
                    }
                    //SelectTrigger(null);
                    _eventEditorContext = new EventEditorContext(this, _overviewContext, selectedRoot, triggers, accessor.GlobalSymbolTable);

                    _variableView.Context = _eventEditorContext;
                    _overviewTab.Context = _eventEditorContext;
                    _triggerEditor.Context = _eventEditorContext;
                }
            }
        }

        if (_eventEditorContext.TriggerRoot)
        {
            _eventEditorContext.TriggerRoot.gameObject.hideFlags = HideFlags.None;
            _eventEditorContext.TriggerRoot.hideFlags = HideFlags.None;
        }
    }

    private void PopulateTriggerList(Transform node, List<Trigger> triggers)
    {
        foreach (Transform child in node.gameObject.transform)
        {
            Trigger trigger = child.GetComponent<Trigger>();
            if (trigger)
            {
                triggers.Add(trigger);
            }

            PopulateTriggerList(child, triggers);
        }
    }
}
