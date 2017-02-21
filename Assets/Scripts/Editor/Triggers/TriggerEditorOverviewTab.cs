using System.Collections.Generic; 
using UnityEditor;
using UnityEngine;

public class OverviewEditorContext
{
    private HashSet<GameObject> _contracted = new HashSet<GameObject>();

    public HashSet<GameObject> ContractedNodes
    {
        get { return _contracted; }
    }

    public GameObject CurrentlyRenaming
    {
        get;
        set;
    }
}

public class TriggerEditorOverviewTab
{
    private EventEditorContext _context;

    private string _createTriggerName = "";

    public EventEditorContext Context
    {
        get
        {
            return _context;
        }
        set
        {
            _context = value;
        }
    }

    private Vector2 _scrollPosition;

    public void Draw()
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(false));

        _context.Triggers.TriggerCollectionRoot.Draw(0);

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Separator();

        if (GUILayout.Button("Create New Trigger"))
        {
            CreateNewTrigger(_context.TriggerRoot.gameObject, "New Trigger");
            _context.Refresh();
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Identifier: ");
        _createTriggerName = GUILayout.TextField(_createTriggerName);
        GUILayout.EndHorizontal();
    }

    private GameObject CreateNewTrigger(GameObject parent, string name)
    {
        GameObject triggerGameObject = new GameObject(name);
        triggerGameObject.AddComponent<Trigger>();
        triggerGameObject.transform.parent = parent.transform;

        return triggerGameObject;
    }

    /*
    protected void ExampleDragDropGUI(Rect dropArea, SerializedProperty property)
    {
        Event currentEvent = Event.current;
        EventType currentEventType = currentEvent.type;

        // The DragExited event does not have the same mouse position data as the other events,
        // so it must be checked now:
        if (currentEventType == EventType.DragExited) DragAndDrop.PrepareStartDrag();
        if (!dropArea.Contains(currentEvent.mousePosition)) return;

        switch (currentEventType)
        {
            case EventType.MouseDown:
                DragAndDrop.PrepareStartDrag();// reset data
                CustomDragData dragData = new CustomDragData();
                dragData.originalIndex = somethingYouGotFromYourProperty;
                dragData.originalList = this.targetList;

                DragAndDrop.SetGenericData(dragDropIdentifier, dragData);

                Object[] objectReferences = new Object[1] { property.objectReferenceValue };
                DragAndDrop.objectReferences = objectReferences;
                currentEvent.Use();
                break;

            case EventType.MouseDrag:
                // If drag was started here:
                CustomDragData existingDragData = DragAndDrop.GetGenericData(dragDropIdentifier) as CustomDragData;

                if (existingDragData != null)
                {
                    DragAndDrop.StartDrag("Dragging List ELement");
                    currentEvent.Use();
                }
                break;
            case EventType.DragUpdated:
                if (IsDragTargetValid()) DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                else DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                currentEvent.Use();
                break;
            case EventType.Repaint:
                if (DragAndDrop.visualMode == DragAndDropVisualMode.None || DragAndDrop.visualMode == DragAndDropVisualMode.Rejected) break;
                EditorGUI.DrawRect(dropArea, Color.grey);
                break;
            case EventType.DragPerform:
                DragAndDrop.AcceptDrag();
                CustomDragData receivedDragData = DragAndDrop.GetGenericData(dragDropIdentifier) as CustomDragData;

                if (receivedDragData != null && receivedDragData.originalList == this.targetList)
                {
                    ReorderObject();
                }
                else
                {
                    AddDraggedObjectsToList();
                }
                currentEvent.Use();
                break;
            case EventType.MouseUp:
                DragAndDrop.PrepareStartDrag();
                break;
        }
    }*/
}
