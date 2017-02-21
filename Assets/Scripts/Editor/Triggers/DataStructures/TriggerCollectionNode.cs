using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class TriggerCollectionNode : IEnumerable<TriggerCollectionNode>
{
    private static readonly GUIContent[] _rootContextOptions = new GUIContent[] { new GUIContent("New Trigger"), new GUIContent("New Group") };
    private static readonly GUIContent[] _triggerContextOptions = new GUIContent[] { new GUIContent("New Trigger"), new GUIContent("Rename"), new GUIContent("Delete") };
    private static readonly GUIContent[] _folderContextOptions = new GUIContent[] { new GUIContent("New Trigger"), new GUIContent("New Group"), new GUIContent("Rename"), new GUIContent("Delete") };

    // TODO add Dictionary<GameObject, bool> for expanded state to OverviewEditorContext.
    // TODO maybe write out references to temp file by ID?
    private EventEditorContext _context;
    private OverviewEditorContext _overviewContext;
    private List<TriggerCollectionNode> _children = new List<TriggerCollectionNode>();

    public bool HasChildren
    {
        get { return _children.Count > 0; }
    }

    public GameObject GameObject
    {
        get;
        private set;
    }

    public Trigger Trigger
    {
        get;
        private set;
    }

    public TriggerCollectionNode(EventEditorContext context, OverviewEditorContext overviewContext, GameObject gameObject)
    {
        GameObject = gameObject;
        if (gameObject.GetComponent<Trigger>())
        {
            Trigger = gameObject.GetComponent<Trigger>();
        }
        _context = context;
        _overviewContext = overviewContext;
    }

    public void AddChild(TriggerCollectionNode childNode)
    {
        _children.Add(childNode);
        _children.Sort((n1, n2) =>
        {
            if (n1.Trigger == null && n2.Trigger == null || n1.Trigger != null && n2.Trigger != null)
            {
                return n1.GameObject.name.CompareTo(n2.GameObject.name);
            }

            return n1.Trigger == null ? -1 : 1;
        });
    }

    public void Draw(int depth)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(15 * depth);

        if (HasChildren)
        {
            bool contracted = _overviewContext.ContractedNodes.Contains(GameObject);
            if (GUILayout.Button(contracted ? "►" : "▼", GUI.skin.label, GUILayout.ExpandWidth(false)))
            {
                if (contracted)
                {
                    _overviewContext.ContractedNodes.Remove(GameObject);
                    //_context.Repaint();
                }
                else
                {
                    _overviewContext.ContractedNodes.Add(GameObject);
                    //_context.Repaint();
                }
            }
        }
        else
        {
            EditorGUILayoutExt.BeginLabelStyle(null, null, new Color(0.35f, 0.35f, 0.35f, 1.0f), null);
            GUILayout.Button("▼", GUI.skin.label, GUILayout.ExpandWidth(false));
            EditorGUILayoutExt.EndLabelStyle();
        }

        if (Trigger)
        {
            DrawTriggerNode();
        }
        else if (GameObject)
        {
            DrawFolderNode();
        }

        EditorGUILayout.EndHorizontal();

        if (!_overviewContext.ContractedNodes.Contains(GameObject))
        {
            foreach (TriggerCollectionNode child in this)
            {
                child.Draw(depth + 1);
            }
        }
    }

    private void DrawFolderNode()
    {
        Color defaultColor = GUI.skin.label.normal.textColor;
        GUI.skin.label.fontStyle = GameObject.activeInHierarchy ? FontStyle.Normal : FontStyle.Italic;
        GUI.skin.label.normal.textColor = GameObject.activeInHierarchy ? defaultColor : new Color(0.55f, 0.55f, 0.55f, 1.0f);
        GameObject.SetActive(GUILayout.Toggle(GameObject.activeSelf, "", GUILayout.ExpandWidth(false)));


        if (this.GameObject == _overviewContext.CurrentlyRenaming)
        {
            GameObject.name = GUILayout.TextField(GameObject.name, GUILayout.ExpandWidth(false));
            if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
            {
                _overviewContext.CurrentlyRenaming = null;
                _context.Refresh();
                Event.current.Use();
            }
        }
        else
        {
            bool useEvent = false;
            GUI.SetNextControlName("FakeButton" + GameObject.GetInstanceID());
            GUILayout.Label(GameObject.name, GUI.skin.label, GUILayout.ExpandWidth(false));
            Rect labelRect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.DragUpdated && labelRect.Contains(Event.current.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                DragAndDrop.AcceptDrag();
                useEvent = true;
            }
            if (Event.current.type == EventType.DragPerform && labelRect.Contains(Event.current.mousePosition))
            {
                if (DragAndDrop.objectReferences.Length == 1 && DragAndDrop.objectReferences[0] is GameObject)
                {
                    GameObject dragged = DragAndDrop.objectReferences[0] as GameObject;
                    dragged.transform.parent = GameObject.transform;
                    _context.Refresh();
                    useEvent = true;
                }
            }
            if (Event.current.type == EventType.MouseDrag && labelRect.Contains(Event.current.mousePosition))
            {
                if (_buttonDown)
                {
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.StartDrag("Draggingwtf?");
                    DragAndDrop.objectReferences = new UnityEngine.Object[] { GameObject };
                    useEvent = true;
                }
            }
            if (Event.current.type == EventType.MouseDown && labelRect.Contains(Event.current.mousePosition))
            {
                _buttonDown = true;
                GUI.FocusControl("FakeButton" + GameObject.GetInstanceID());
                useEvent = true;
            }
            if (_buttonDown && Event.current.type == EventType.MouseUp && labelRect.Contains(Event.current.mousePosition))
            {
                _buttonDown = false;
                _buttonPressed = true;
                useEvent = true;
            }
            if (_buttonPressed && Event.current.type == EventType.MouseUp && labelRect.Contains(Event.current.mousePosition))
            {
                _buttonPressed = false;
                if (Event.current.button == 1)
                {
                    Vector2 mousePosition = Event.current.mousePosition;
                    if (this == _context.Triggers.TriggerCollectionRoot)
                    {
                        EditorUtility.DisplayCustomMenu(new Rect(mousePosition.x, mousePosition.y - 300, 100, 300), _rootContextOptions, -1, RootContextMenuFunction, this);
                    }
                    else
                    {
                        EditorUtility.DisplayCustomMenu(new Rect(mousePosition.x, mousePosition.y - 300, 100, 300), _folderContextOptions, -1, FolderContextMenuFunction, this);
                    }
                }
                useEvent = true;
            }
            if (useEvent)
            {
                _context.Repaint = true;
                Event.current.Use();
            }
        }

        GUI.skin.label.fontStyle = FontStyle.Normal;
        GUI.skin.label.normal.textColor = defaultColor;
    }

    private bool _buttonDown = false;
    private bool _buttonPressed = false;

    private void DrawTriggerNode()
    {
        Color defaultColor = GUI.skin.button.normal.textColor;
        GUI.skin.button.fontStyle = GameObject.activeInHierarchy ? FontStyle.Normal : FontStyle.Italic;
        GUI.skin.button.normal.textColor = GameObject.activeInHierarchy ? defaultColor : new Color(0.55f, 0.55f, 0.55f, 1.0f);

        Trigger.Enabled = GUILayout.Toggle(Trigger.Enabled, "", GUILayout.ExpandWidth(false));
        if (this.GameObject == _overviewContext.CurrentlyRenaming)
        {
            GameObject.name = GUILayout.TextField(GameObject.name, GUILayout.ExpandWidth(false));
            if (Event.current.isKey && Event.current.keyCode == KeyCode.Return || Event.current.type == EventType.MouseDown)// && !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                _overviewContext.CurrentlyRenaming = null;
                _context.Refresh();
                Event.current.Use();
            }
        }
        else
        {
            if (_context.SelectedTrigger == Trigger)
            {
                GUI.skin.button.normal.textColor = new Color(1.0f, 0.45f, 0.45f, 1.0f);
                GUI.SetNextControlName("FakeButton" + GameObject.GetInstanceID());
                GUILayout.Label(GameObject.name, GUI.skin.button, GUILayout.ExpandWidth(false));
            }
            else
            {
                GUI.skin.button.fontStyle = FontStyle.Normal;
                GUI.skin.button.normal.textColor = defaultColor;
                GUI.SetNextControlName("FakeButton" + GameObject.GetInstanceID());
                GUILayout.Label(GameObject.name, GUI.skin.button, GUILayout.ExpandWidth(false));
            }
            bool useEvent = false;
            Rect labelRect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.MouseDrag && labelRect.Contains(Event.current.mousePosition))
            {
                if (_buttonDown)
                {
                    _buttonDown = false;
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.StartDrag("Draggingwtf?");
                    DragAndDrop.objectReferences = new UnityEngine.Object[] { GameObject };
                    useEvent = true;
                }
            }
            if (Event.current.type == EventType.MouseDown && labelRect.Contains(Event.current.mousePosition))
            {
                _buttonDown = true;
                useEvent = true;
                GUI.FocusControl("FakeButton" + +GameObject.GetInstanceID());
            }
            if (_buttonDown && Event.current.type == EventType.MouseUp && labelRect.Contains(Event.current.mousePosition))
            {
                _buttonDown = false;
                _buttonPressed = true;
                useEvent = true;
            }
            if (_buttonPressed && Event.current.type == EventType.MouseUp && labelRect.Contains(Event.current.mousePosition))
            {
                _buttonPressed = false;
                _context.SelectTrigger(Trigger);
                _context.Repaint = true;
                if (Event.current.button == 1)
                {
                    Vector2 mousePosition = Event.current.mousePosition;
                    EditorUtility.DisplayCustomMenu(new Rect(mousePosition.x, mousePosition.y - 300, 100, 300), _triggerContextOptions, -1, TriggerContextMenuFunction, this);
                }
                useEvent = true;
            }

            if (useEvent)
            {
                _context.Repaint = true;
                Event.current.Use();
            }
        }

        GUI.skin.button.normal.textColor = defaultColor;
        GUI.skin.button.fontStyle = FontStyle.Normal;
    }

    private void RootContextMenuFunction(object obj, string[] selections, int selectionIndex)
    {
        TriggerCollectionNode node = obj as TriggerCollectionNode;
        switch (selections[selectionIndex])
        {
            case "New Trigger":
                CreateNewTrigger(node.GameObject, "New Trigger");
                _context.Refresh();
                break;
            case "New Group":
                CreateNewFolder(node.GameObject, "New Group");
                _context.Refresh();
                break;
            default:
                Debug.LogError("Unknown Trigger Context Menu Selection.");
                break;
        }
    }

    private void TriggerContextMenuFunction(object obj, string[] selections, int selectionIndex)
    {
        TriggerCollectionNode node = obj as TriggerCollectionNode;
        switch (selections[selectionIndex])
        {
            case "New Trigger":
                CreateNewTrigger(node.GameObject.transform.parent.gameObject, "New Trigger");
                _context.Refresh();
                break;
            case "Delete":
                GameObject.DestroyImmediate(node.GameObject);
                _context.Refresh();
                break;
            case "Rename":
                _overviewContext.CurrentlyRenaming = node.GameObject;
                break;
            default:
                Debug.LogError("Unknown Trigger Context Menu Selection.");
                break;
        }
    }

    private void FolderContextMenuFunction(object obj, string[] selections, int selectionIndex)
    {
        TriggerCollectionNode node = obj as TriggerCollectionNode;
        switch (selections[selectionIndex])
        {
            case "New Trigger":
                CreateNewTrigger(node.GameObject, "New Trigger");
                _context.Refresh();
                break;
            case "New Group":
                CreateNewFolder(node.GameObject, "New Group");
                _context.Refresh();
                break;
            case "Delete":
                GameObject.DestroyImmediate(node.GameObject);
                _context.Refresh();
                break;
            case "Rename":
                _overviewContext.CurrentlyRenaming = node.GameObject;
                break;
            default:
                Debug.LogError("Unknown Trigger Context Menu Selection.");
                break;
        }
    }

    private GameObject CreateNewTrigger(GameObject parent, string name)
    {
        GameObject triggerGameObject = new GameObject(name);
        triggerGameObject.AddComponent<Trigger>();
        triggerGameObject.transform.parent = parent.transform;

        return triggerGameObject;
    }

    private GameObject CreateNewFolder(GameObject parent, string name)
    {
        GameObject triggerGameObject = new GameObject(name);
        triggerGameObject.transform.parent = parent.transform;

        return triggerGameObject;
    }

    public IEnumerator<TriggerCollectionNode> GetEnumerator()
    {
        List<TriggerCollectionNode> toDelete = new List<TriggerCollectionNode>();
        foreach (TriggerCollectionNode child in _children)
        {
            if (!child.GameObject)
            {
                toDelete.Add(child);
                continue;
            }
            yield return child;
        }

        foreach (TriggerCollectionNode deleted in toDelete)
        {
            _children.Remove(deleted);
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
