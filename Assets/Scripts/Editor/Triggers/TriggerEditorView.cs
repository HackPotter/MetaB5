using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TriggerEditorView
{
    private EventEditorContext _context;
    private TriggerTree _selectedTriggerTree;

    private Vector2 _scrollPosition;

    private bool _commentExpanded = true;

    public EventEditorContext Context
    {
        get { return _context; }
        set
        {
            _context = value;
            if (_selectedTriggerTree != null)
            {
                _selectedTriggerTree.EventNode.Context = value;
            }
        }
    }

    public void SelectTrigger(Trigger trigger)
    {
        if (_selectedTriggerTree == null && trigger == null)
        {
            // do nothing
        }
        else if (_selectedTriggerTree == null && trigger != null)
        {
            _selectedTriggerTree = BuildTree(trigger);
        }
        else if (_selectedTriggerTree != null && trigger == null)
        {
            _selectedTriggerTree.Delete();
            _selectedTriggerTree = null;
        }
        else if (_selectedTriggerTree.Trigger != trigger)
        {
            _selectedTriggerTree.Delete();
            _selectedTriggerTree = BuildTree(trigger);
        }
        else if (_selectedTriggerTree.Trigger == trigger)
        {
            TriggerTree newTree = BuildTree(trigger);
            TriggerTree oldTree = _selectedTriggerTree;

            SyncTrees(oldTree, newTree);
            _selectedTriggerTree = newTree;
            oldTree.Delete();
        }
    }

    private void SyncTrees(TriggerTree oldTree, TriggerTree newTree)
    {
        Dictionary<MonoBehaviour, TriggerEditorNode> currentState = new Dictionary<MonoBehaviour, TriggerEditorNode>();
        GetStateByNode(oldTree.EventNode, currentState);
        SyncNodeState(newTree.EventNode, currentState);
    }

    private void SyncNodeState(TriggerEditorNode node, Dictionary<MonoBehaviour, TriggerEditorNode> state)
    {
        if (node.TriggerComponent != null && state.ContainsKey(node.TriggerComponent))
        {
            node.Expanded = state[node.TriggerComponent].Expanded;
        }

        foreach (TriggerEditorNode child in node)
        {
            SyncNodeState(child, state);
        }
    }

    private void GetStateByNode(TriggerEditorNode node, Dictionary<MonoBehaviour, TriggerEditorNode> state)
    {
        if (node.TriggerComponent != null)
        {
            state.Add(node.TriggerComponent, node);
        }
        foreach (var child in node)
        {
            GetStateByNode(child, state);
        }
    }

    private TriggerTree BuildTree(Trigger trigger)
    {
        TriggerTree tree = new TriggerTree();
        tree.Trigger = trigger;
        tree.EventNode = new EventEditorNode(trigger.gameObject, _context);
        List<TriggerEditorNode> children = GetChildrenNodes(trigger.gameObject);

        foreach (TriggerEditorNode child in children)
        {
            tree.EventNode.Add(child);
        }

        return tree;
    }

    private List<TriggerEditorNode> GetChildrenNodes(GameObject root)
    {
        // Root is assumed to contain a Trigger component and maybe an EventSender or Function component.
        List<TriggerEditorNode> nodes = new List<TriggerEditorNode>();

        foreach (Transform child in root.transform)
        {
            EventFilter filter = child.GetComponent<EventFilter>();
            EventResponder responder = child.GetComponent<EventResponder>();
            TriggerActionGroup actionGroup = child.GetComponent<TriggerActionGroup>();
            if (filter)
            {
                FilterEditorNode filterNode = new FilterEditorNode(filter, _context);
                nodes.Add(filterNode);
                BuildTreeRec(filterNode);
            }
            else if (responder)
            {
                ActionEditorNode actionNode = new ActionEditorNode(responder, _context);
                nodes.Add(actionNode);
                BuildTreeRec(actionNode);
            }
            else if (actionGroup)
            {
                ActionGroupEditorNode groupNode = new ActionGroupEditorNode(actionGroup, _context);
                nodes.Add(groupNode);
                BuildTreeRec(groupNode);
            }
        }
        return nodes;
    }

    private void BuildTreeRec(TriggerEditorNode node)
    {
        foreach (Transform child in node.TriggerComponent.transform)
        {
            EventFilter filter = child.GetComponent<EventFilter>();
            EventResponder responder = child.GetComponent<EventResponder>();
            TriggerActionGroup actionGroup = child.GetComponent<TriggerActionGroup>();
            if (filter)
            {
                FilterEditorNode filterNode = new FilterEditorNode(filter, _context);
                node.Add(filterNode);
                BuildTreeRec(filterNode);
            }
            else if (responder)
            {
                ActionEditorNode actionNode = new ActionEditorNode(responder, _context);
                node.Add(actionNode);
                BuildTreeRec(actionNode);
            }
            else if (actionGroup)
            {
                ActionGroupEditorNode groupNode = new ActionGroupEditorNode(actionGroup, _context);
                node.Add(groupNode);
                BuildTreeRec(groupNode);
            }
        }
    }

    public void Draw()
    {
        SelectTrigger(_context.SelectedTrigger);
        if (_selectedTriggerTree == null)
            return;
        //if (_selectedTriggerTree == null || !_selectedTriggerTree.Trigger)
        //{
        //    if (_context.SelectedTrigger)
                
        //        _selectedTriggerTree = BuildTree(_context.SelectedTrigger);
        //    else
        //        return;
        //}

        //if (_selectedTriggerTree.Trigger != _context.SelectedTrigger)
        //    _selectedTriggerTree = BuildTree(_context.SelectedTrigger);

        if (_selectedTriggerTree != null)
        {
            GUILayout.BeginHorizontal();
            bool newEnabled = GUILayout.Toggle(_selectedTriggerTree.Trigger.Enabled, "", GUILayout.ExpandWidth(false));
            if (newEnabled != _selectedTriggerTree.Trigger.Enabled)
            {
                _selectedTriggerTree.Trigger.Enabled = newEnabled;
            }
            string newName = GUILayout.TextField(_selectedTriggerTree.Trigger.name);
            if (newName != _selectedTriggerTree.Trigger.name)
            {
                _selectedTriggerTree.Trigger.name = newName;
            }
            GUILayout.EndHorizontal();

            _commentExpanded = GUILayout.Button(_commentExpanded ? "▼ Comment" : "► Comment", GUI.skin.label, GUILayout.ExpandWidth(false)) ^ _commentExpanded;
            if (_commentExpanded)
            {
                string newComment = GUILayout.TextArea(_selectedTriggerTree.Trigger.Comment);
                if (newComment != _selectedTriggerTree.Trigger.Comment)
                {
                    _selectedTriggerTree.Trigger.Comment = newComment;
                }
            }
            else
            {
                string text = _selectedTriggerTree.Trigger.Comment.Split('\n')[0];
                _commentExpanded = GUILayout.Button(text, GUI.skin.label, GUILayout.ExpandWidth(true)) ^ _commentExpanded;
            }
            EditorGUILayout.Separator();

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            _selectedTriggerTree.EventNode.OnGUI(0);

            GUILayout.EndScrollView();
        }
    }

    private class TriggerTree
    {
        public Trigger Trigger
        {
            get;
            set;
        }

        public EventEditorNode EventNode
        {
            get;
            set;
        }

        public TriggerTree()
        {
        }

        public void Delete()
        {
            EventNode.Delete();
        }
    }
}
